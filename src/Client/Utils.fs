module Visibility

open Todo.Types

let parse = function
    | nameof Visibility.All -> Some Visibility.All
    | nameof Visibility.Completed -> Some Visibility.Completed
    | nameof Visibility.NotCompleted -> Some Visibility.NotCompleted
    | _ -> None