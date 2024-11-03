using AwaraIT.Training.Domain;
using AwaraIT.Training.Domain.Extensions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AwaraIT.Training.Infrastucture.Sources.Crm
{
    public abstract class CrmBaseClient<T> where T : Entity
    {
        public IOrganizationService Service { get; private set; }

        protected abstract string EntityName { get; }

        public CrmBaseClient(IOrganizationService service)
        {
            Service = service;
        }

        public Guid Create(T entity)
        {
            return Service.Create(entity.ToEntity<Entity>());
        }

        public void Update(T entity)
        {
            if (entity.Attributes.Count > 0)
            {
                Service.Update(entity.ToEntity<Entity>());
            }
        }

        public void Delete(Guid id)
        {
            Service.Delete(EntityName, id);
        }

        public T Get(Guid id, bool allColumns)
        {
            return Service.Retrieve(EntityName, id, new ColumnSet(allColumns)).ToEntity<T>();
        }

        public T Get(Guid id, params string[] attributes)
        {
            return Service.Retrieve(EntityName, id, new ColumnSet(attributes)).ToEntity<T>();
        }

        public List<T> Get(IEnumerable<ConditionExpression> conditions, int? topCount = null, params string[] columns)
        {
            var query = new QueryExpression(EntityName)
            {
                NoLock = true
            };

            if (topCount != null)
            {
                query.TopCount = topCount.Value;
            }

            if (conditions.HasItems())
            {
                query.Criteria = new FilterExpression();
                foreach (var condition in conditions)
                {
                    query.Criteria.AddCondition(condition);
                }
            }

            if (columns != null)
            {
                query.ColumnSet = new ColumnSet(columns);
            }
            else
            {
                query.ColumnSet = new ColumnSet(true);
            }

            var entities = Service.RetrieveMultiple(query);

            return entities.Entities.Select(e => e.ToEntity<T>()).ToList();
        }

        public List<T> Get(QueryExpression query)
        {
            return Service.RetrieveMultiple(query).Entities
                .Select(e => e.ToEntity<T>())
                .ToList();
        }

        public List<T> GetAllPaged(QueryExpression query, int pageSize = Constants.MaxPageSize)
        {
            var result = new List<Entity>();
            query.PageInfo = new PagingInfo
            {
                Count = pageSize,
                PageNumber = 1,
                PagingCookie = null
            };

            while (true)
            {
                // Retrieve the page.
                var results = Service.RetrieveMultiple(query);
                result.AddRange(results.Entities);

                if (results.MoreRecords)
                {
                    query.PageInfo.PageNumber++;
                    query.PageInfo.PagingCookie = results.PagingCookie;
                }
                else
                {
                    break;
                }
            }

            return result.Select(e => e.ToEntity<T>()).ToList();
        }

        public OrganizationResponse Execute(OrganizationRequest request)
        {
            return Service.Execute(request);
        }
    }
}
