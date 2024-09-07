// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Performance", "CA1860:Avoid using 'Enumerable.Any()' extension method", Justification = "Entity framework translates 'Enumerable.Any()' more efficiently than the count alternative.", Scope = "member", Target = "~M:FitnessTracker.Controllers.UserController.CreateWorkoutPin(FitnessTracker.DTOs.Requests.User.CreatePinsRequestDTO)~System.Threading.Tasks.Task{Microsoft.AspNetCore.Mvc.IActionResult}")]
