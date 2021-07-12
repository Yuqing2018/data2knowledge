using Microsoft.Extensions.DependencyInjection;
using MusicKG.DataAccess.Models;
using MusicKG.DataManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicKG.HondaPlugins.OperationTool.CommandActions
{
    public class CommandExecutor
    {
        public static bool OperateModel(ModelOptions options, IServiceProvider services)
        {
            if (!options.Check())
            {
                Console.WriteLine("Invalid parameters for model options, please check honda-ops --help to find details.");
                return false;
            }

            using (var scope = services.CreateScope())
            {
                var modelOperator = scope.ServiceProvider.GetRequiredService<IModelOperator>();

                var modelName = options.ModelName;
                var version = options.Version;

                if (options.List)
                {
                    var models = modelOperator.ListAsync(modelName?.ToString()).GetAwaiter().GetResult();

                    ShowModels(models);
                }

                if (options.Train)
                {
                    modelOperator.TrainAsync(modelName.Value.ToString()).GetAwaiter().GetResult();
                    Console.WriteLine("Train model succeed!");
                }

                if (options.Revert)
                {
                    modelOperator.RevertAsync(modelName.Value.ToString(), version.Value).GetAwaiter().GetResult();
                    Console.WriteLine("Revert model succeed!");
                }

                return true;
            }
        }

        public static bool OperateUser(UserOptions options, IServiceProvider services)
        {
            if (!options.Check())
            {
                Console.WriteLine("Invalid parameters for user options, please check honda-ops --help to find details.");
                return false;
            }

            using (var scope = services.CreateScope())
            {
                try
                {
                    var userOperator = scope.ServiceProvider.GetRequiredService<IUserOperator>();

                    var userName = options.UserName;
                    var password = options.Password;
                    var newName = options.NewName;

                    if (options.List)
                    {
                        var users = userOperator.ListAsync(userName).GetAwaiter().GetResult();

                        ShowUsers(users);
                    }

                    if (options.Create)
                    {
                        userOperator.CreateAsync(userName, password).GetAwaiter().GetResult();
                        Console.WriteLine("Create user succeed!");
                    }

                    if (options.Disable)
                    {
                        userOperator.DisableAsync(userName).GetAwaiter().GetResult();
                        Console.WriteLine("Disable user succeed!");
                    }

                    if (options.Enable)
                    {
                        userOperator.EnableAsync(userName).GetAwaiter().GetResult();
                        Console.WriteLine("Enable user succeed!");
                    }

                    if (options.ResetPassword)
                    {
                        userOperator.ResetPasswordAsync(userName, password).GetAwaiter().GetResult();
                        Console.WriteLine("Reset user password succeed!");
                    }

                    if (options.Rename)
                    {
                        userOperator.RenameAsync(userName, newName).GetAwaiter().GetResult();
                        Console.WriteLine("Rename user succeed!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
                
                return true;
            }
        }

        private static void ShowUsers(List<UserDataModel> users)
        {
            var lines = users.Select(user =>
            {
                return new List<string>
                {
                    user.Name, user.Status.ToString(), user.CreatedAt.ToString()
                };
            }).ToList();

            lines.Insert(0, new List<string> { "USER NAME", "USER STATUS", "CREATED DATE" });

            var column1Max = lines.Max(x => x[0].Length);
            var column2Max = lines.Max(x => x[1].Length);
            var column3Max = lines.Max(x => x[2].Length);

            lines.ForEach(row =>
            {
                row[0] = row[0].PadRight(column1Max);
                row[1] = row[1].PadRight(column2Max);
                row[2] = row[2].PadRight(column3Max);
            });

            lines.ForEach(row => Console.WriteLine($"{row[0]}\t{row[1]}\t{row[2]}"));
        }

        private static void ShowModels(List<ModelTrainingHistory> models)
        {
            var lines = models.GroupBy(x => $"{x.ModelName}_{x.CurrentVersion}").ToDictionary(k => k.Key, v => v.SelectMany(h => h.ModelVersions).ToList())
                .SelectMany(h =>
                {
                    return h.Value.Select((v, index) =>
                    {
                        var modelName = h.Key.Split("_")[0];
                        var currentVersion = h.Key.Split("_")[1];
                        var version = v.Version.ToString() == currentVersion ? $"{v.Version} (Current)" : v.Version.ToString();
                        if (index == 0)
                            return new List<string> { modelName, version, v.TrainedAt.ToString() };
                        return new List<string> { "", version, v.TrainedAt.ToString() };
                    });
                }).ToList();

            lines.Insert(0, new List<string> { "MODEL NAME", "MODEL VERSION", "TRAINED DATE" });

            var column1Max = lines.Max(x => x[0].Length);
            var column2Max = lines.Max(x => x[1].Length);
            var column3Max = lines.Max(x => x[2].Length);

            lines.ForEach(row =>
            {
                row[0] = row[0].PadRight(column1Max);
                row[1] = row[1].PadRight(column2Max);
                row[2] = row[2].PadRight(column3Max);
            });

            lines.ForEach(row => Console.WriteLine($"{row[0]}\t{row[1]}\t{row[2]}"));
        }
    }
}
