using MongoDB.Bson;
using MongoDB.Driver;
using MusicKG.DataAccess.Enums;
using MusicKG.DataAccess.Models;
using MusicKG.Workflow.Models.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Extensions
{
    public static class WorkflowStepInputFilterExtensions
    {
        public static FilterDefinition<DocumentDataModel> ToMongoFilter(this WorkflowStepInputFilterServiceModel filter)
        {
            var builder = Builders<DocumentDataModel>.Filter;

            var workflowTypeFilter = builder.Empty;
            var statusFilter = builder.Empty;
            var birthStepFilter = builder.Empty;
            var nextStepFilter = builder.Empty;
            
            workflowTypeFilter = builder.Eq(x => x.WorkflowId, new ObjectId(filter.WorkflowId));

            if (filter.Status != null)
            {
                foreach (var status in filter.Status)
                {
                    if (statusFilter == builder.Empty)
                    {
                        statusFilter = builder.Eq(x => x.Status, status);
                    }
                    else
                    {
                        statusFilter |= builder.Eq(x => x.Status, status);
                    }
                }
            }
            
            if (filter.BirthStep != null)
            {
                birthStepFilter = filter.BirthStep.ToMongoFilter();
            }

            nextStepFilter = builder.Eq(x => x.NextStep, null);
            if (filter.DeathStep != null)
            {
                nextStepFilter |= filter.DeathStep.ToMongoFilter();
            }

            return workflowTypeFilter & statusFilter & birthStepFilter & nextStepFilter;
        }

        public static FilterDefinition<DocumentDataModel> ToMongoFilter(this BirthStepFilterDataModel birthStepFilter)
        {
            var builder = Builders<DocumentDataModel>.Filter;

            var statusFilter = builder.Empty;
            var stepFilter = builder.Empty;

            var status = birthStepFilter.Status;
            var steps = birthStepFilter.Steps;

            if (status != null && status.Count > 0)
            {
                foreach (var state in status)
                {
                    if (statusFilter == builder.Empty)
                    {
                        statusFilter = builder.Eq(x => x.BirthStep.Status, state);
                    }
                    else
                    {
                        statusFilter |= builder.Eq(x => x.BirthStep.Status, state);
                    }
                }
            }

            if (steps != null && steps.Count > 0)
            {
                foreach (var step in steps)
                {
                    if (stepFilter == builder.Empty)
                    {
                        stepFilter = builder.Eq(x => x.BirthStep.StepId, step);
                    }
                    else
                    {
                        stepFilter |= builder.Eq(x => x.BirthStep.StepId, step);
                    }
                }
            }

            return statusFilter & stepFilter;
        }

        public static FilterDefinition<DocumentDataModel> ToMongoFilter(this DeathStepDataModel deathStepFilter)
        {
            var builder = Builders<DocumentDataModel>.Filter;

            var statusFilter = builder.Empty;
            var stepFilter = builder.Empty;
            var timesFilter = builder.Empty;

            var status = deathStepFilter.Status;
            var steps = deathStepFilter.Steps;

            if (status != null && status.Count > 0)
            {
                foreach (var state in status)
                {
                    if (statusFilter == builder.Empty)
                    {
                        statusFilter = builder.Eq(x => x.NextStep.Status, state);
                    }
                    else
                    {
                        statusFilter |= builder.Eq(x => x.NextStep.Status, state);
                    }
                }
            }

            if (steps != null && steps.Count > 0)
            {
                foreach (var step in steps)
                {
                    if (stepFilter == builder.Empty)
                    {
                        stepFilter = builder.Eq(x => x.NextStep.StepId, step);
                    }
                    else
                    {
                        stepFilter |= builder.Eq(x => x.NextStep.StepId, step);
                    }
                }
            }

            if (deathStepFilter.MaxRetryTimes > 0)
            {
                timesFilter = builder.Lt(x => x.NextStep.Times, deathStepFilter.MaxRetryTimes);
            }

            return statusFilter & stepFilter & timesFilter;
        }
    }
}
