﻿using Microsoft.EntityFrameworkCore;
using FitnessTracker.Data;
using FitnessTracker.Models;
using System.Linq.Expressions;

namespace FitnessTracker.Services.Read
{
    public class WorkoutReadService(DataContext context) : AbstractReadService<Workout>(context)
    {
        protected override Expression<Func<Workout, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (int.TryParse(value, out int valueId))
            {
                return key switch
                {
                    "equipment" => x => x.Sets.Any(x => x.Exercise.Equipment.Any(x => x.Id == valueId)),
                    "primarymusclegroup" => x => x.Sets.Any(x => x.Exercise.PrimaryMuscleGroups.Any(x => x.Id == valueId)),
                    "secondarymusclegroup" => x => x.Sets.Any(x => x.Exercise.SecondaryMuscleGroups.Any(x => x.Id == valueId)),
                    "primarymuscle" => x => x.Sets.Any(x => x.Exercise.PrimaryMuscles.Any(x => x.Id == valueId)),
                    "secondarymuscle" => x => x.Sets.Any(x => x.Exercise.SecondaryMuscles.Any(x => x.Id == valueId)),
                    _ => throw new NotSupportedException($"Invalid key in search query. Entered key: {key}")
                };
            }

            if (value.Contains(','))
            {
                var values = value.Replace(" ", "").Split(',');
                List<int> valueIds = [];
                foreach (var id in values)
                {
                    if (int.TryParse(id, out int newId))
                        valueIds.Add(newId);
                }

                return key switch
                {
                    "primarymusclegroup" => x => x.Sets.Any(x => x.Exercise.PrimaryMuscleGroups.Any(m => valueIds.Contains(m.Id))),
                    "secondarymusclegroup" => x => x.Sets.Any(x => x.Exercise.SecondaryMuscleGroups.Any(m => valueIds.Contains(m.Id))),
                    "primarymuscle" => x => x.Sets.Any(x => x.Exercise.PrimaryMuscles.Any(m => valueIds.Contains(m.Id))),
                    "secondarymuscle" => x => x.Sets.Any(x => x.Exercise.SecondaryMuscles.Any(m => valueIds.Contains(m.Id))),
                    "equipment" => x => x.Sets.Any(x => x.Exercise.Equipment.Any(eq => valueIds.Contains(eq.Id))),
                    _ => throw new NotSupportedException($"Invalid key in search query. Entered key: {key}"),
                };
            }

            return key switch
            {
                "personal" => x => true,
                "user" => x => true,
                "userid" => x => true,
                _ => throw new NotSupportedException($"Invalid key in search query. Entered key: {key}"),
            };
        }

        public override async Task<List<Workout>> Get(string? query, int? offset = 0, int? limit = -1, string? include = "all")
        {
            return await Task.Run(() =>
            {
                var entitiesQueryable = GetIncluded(include);
                if (query is null)
                    return ApplyOffsetAndLimit(entitiesQueryable, offset, limit).Where(x => x.IsPublic).ToList();

                var keyValuePairsInSearchQuery = SplitQueryString(query);
                List<string>? strictKeyValuePair = keyValuePairsInSearchQuery.FirstOrDefault(kvp => kvp[0] == "strict");
                bool isStrictModeEnabled = false;

                if (strictKeyValuePair != null)
                {
                    isStrictModeEnabled = strictKeyValuePair[1] == "true";
                    keyValuePairsInSearchQuery.Remove(strictKeyValuePair);
                }

                List<Workout> entities = [];
                if (isStrictModeEnabled)
                {
                    foreach (var criteria in DecipherQuery(keyValuePairsInSearchQuery))
                        entitiesQueryable = entitiesQueryable.Where(criteria);

                    entities = ApplyOffsetAndLimit(entitiesQueryable, offset, limit);
                }
                else
                {
                    entities = ApplyOffsetAndLimit(ApplyNonStrictCriterias(entitiesQueryable, DecipherQuery(keyValuePairsInSearchQuery)), offset, limit);
                }



                List<string>? userKeyValuePair = keyValuePairsInSearchQuery.FirstOrDefault(kvp => kvp[0] == "user" || kvp[0] == "userid");
                if (userKeyValuePair != null && Guid.TryParse(userKeyValuePair[1], out var creatorId))
                {
                    List<string>? personalKeyValuePair = keyValuePairsInSearchQuery.FirstOrDefault(kvp => kvp[0] == "personal");
                    if (personalKeyValuePair is null)
                        return entities.Where(x => x.IsPublic && x.CreatorId != creatorId).ToList();
                    else
                    {
                        return personalKeyValuePair[1].ToLower().Trim() == "true"
                            ? entities.Where(x => x.CreatorId == creatorId).ToList()
                            : entities.Where(x => x.IsPublic && x.CreatorId != creatorId).ToList();
                    }
                }
                return entities.Where(x => x.IsPublic).ToList();
            });
        }
    }
}
