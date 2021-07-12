using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Data;
using TaskCreationOptions = MusicKG.DataManager.Translator.Options.TaskCreationOptions;
using MusicKG.DataManager.Models;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Extensions;
using MusicKG.WebApi.ClientWrapper;
using MusicKG.WebApi.Contract.BindingModels;
using MusicKG.WebApi.Contract.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MusicKG.DataManager.Translator.Executors
{
    public class DefaultTaskCreator : ActionExecutor<DataTranslatorContext, TaskCreationOptions, TaskCreationActionData, object>
    {
        public DefaultTaskCreator(ILogger<DefaultTaskCreator> logger) : base(logger)
        {
            ExecutorType = DataTranslationDefaultExecutors.DefaultTaskCreator.ToString();
        }

        protected async override Task<object> ExecuteInternalAsync(string actionId, DataTranslatorContext context, TaskCreationOptions options, TaskCreationActionData data)
        {
            if (data == null || data.Documents == null || context.DataCount <= 0)
            {
                logger.LogActionInfo(actionId, "There is no document waiting for tasking.");
                return null;
            }

            if (options.Ignore)
            {
                data.Documents.ToList();
                return null;
            }

            var httpClient = context.HttpClient;

            var autoTaskRule = await TaskCreationRuleWrapper.GetAsync(httpClient, context.ServiceUrl,
                context.Parameters.WorkspaceId, context.Token);

            if (autoTaskRule == null || autoTaskRule.Rules?.Count == 0)
            {
                logger.LogActionWarning(actionId, $"There is no valid rule for workspace '{context.Parameters.WorkspaceId}'.");
                return null;
            }

            var rules = autoTaskRule.Rules;

            var documents = data.Documents?.ToList();

            if (documents?.Count == 0)
            {
                logger.LogActionInfo(actionId, $"There is no document waiting for assigning for workspace '{context.Parameters.WorkspaceId}'.");
                return null;
            }

            var createdDocs = new List<SimpleDocumentModel>();

            foreach (var rule in rules.Where(r => r.DocumentTags != null && r.DocumentTags.Count > 0))
            {
                var documentForRule = documents.Where(document => rule.DocumentTags.Any(t => document.Tags.Contains(t)))?.ToList();

                if (documentForRule?.Count == 0)
                {
                    logger.LogActionInfo(actionId, $"There is no document waiting for assigning for rule '{rule.Name}', skip this rule.");
                    continue;
                }
                else
                {
                    var docs = await CreateTaskAsync(actionId, context, rule, autoTaskRule.OnlyCreateWhanMatchDocumentCount, documentForRule);

                    if (docs != null && docs.Count > 0)
                    {
                        createdDocs.AddRange(docs);
                    }
                }
            }

            var documentsWithoutRules = documents.Except(createdDocs)?.ToList();

            var defaultRule = rules.FirstOrDefault(r => r.DocumentTags == null || r.DocumentTags.Count == 0);

            if (defaultRule != null)
            {
                await CreateTaskAsync(actionId, context, defaultRule, false, documentsWithoutRules);
            }

            return null;
        }

        protected override async Task RevertInternalAsync(string actionId, DataTranslatorContext context)
        {
            if (context.CreatedTasks?.Count == 0)
            {
                logger.LogActionInfo(actionId, "There is no task has been created, no need to revert.");
                return;
            }

            var httpClient = context.HttpClient;

            context.CreatedTasks = context.CreatedTasks.SkipWhile(task =>
            {
                try
                {
                    TaskWrapper.DeleteAsync(httpClient, context.ServiceUrl,
                        context.Parameters.WorkspaceId, task, context.Token).GetAwaiter().GetResult();
                    return true;
                }
                catch
                {
                    logger.LogActionWarning(actionId, $"Delete task {task} failed.");
                    return false;
                }
            })?.ToList();
        }

        private async Task<List<SimpleDocumentModel>> CreateTaskAsync(
            string actionId, DataTranslatorContext context,
            AutoTaskRuleViewModel rule, bool onlyCreateWhenCountMatch,
            List<SimpleDocumentModel> documentForRule)
        {
            var httpClient = context.HttpClient;
            var url = context.ServiceUrl;
            var token = context.Token;
            var workspaceId = context.Parameters.WorkspaceId;

            var usedDocuments = new List<SimpleDocumentModel>();

            for (int i = 0; i < documentForRule.Count; i += rule.DocumentCount)
            {
                var docs = documentForRule.Skip(i).Take(rule.DocumentCount).ToList();

                if (docs.Count < rule.DocumentCount && onlyCreateWhenCountMatch)
                {
                    continue;
                }

                usedDocuments.AddRange(docs);

                var docsString = string.Join(",", docs.Select(d => d.Name));

                var tags = docs.SelectMany(d => d.Tags).ToHashSet();

                var taskName = $"{string.Join("_", tags)}_{GetLocalDate(context.CurrentTimeRange.Start, 8):yyyy-MM-dd HH:mm:ss}_{GetLocalDate(context.CurrentTimeRange.End, 8):yyyy-MM-dd HH:mm:ss}_{i + 1}";

                logger.LogActionInfo(actionId, $"Creating task for workpace '{workspaceId}'. Documents: '{docsString}'. TaskName: '{taskName}'");

                var result = await TaskWrapper.CreateAsync(httpClient, url, workspaceId, new TaskCreateBindingModel
                {
                    IsAutoApproved = rule.IsAutoApproved,
                    IsAutoMerged = rule.IsAutoMerged,
                    AnnotatorIds = rule.Annotators.Select(a => a.ToString()).ToList(),
                    Overlap = rule.Overlap,
                    DocumentIds = docs.Select(d => d.Id).ToList(),
                    ExpectedDueAt = DateTime.UtcNow.Date.AddDays(rule.MaxFinishDays),
                    Name = taskName
                }, token);

                context.CreatedTasks.Add(result);

                logger.LogActionInfo(actionId, $"Task '{result} - {taskName}' for workpace '{workspaceId}' created. Documents: '{docsString}'.");
            }

            return usedDocuments;
        }

        private DateTime GetLocalDate(DateTime time, int utcOffset)
        {
            return time.AddHours(utcOffset);
        }
    }
}
