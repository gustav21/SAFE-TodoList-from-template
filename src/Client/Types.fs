module Todo.Types

open Shared
open System

type Visibility =
  | All
  | Completed
  | NotCompleted

type State = {
  TodoItems: List<Todo>
  NewTodoDescription: Option<string>
  NewTodoDueDate: Option<DateTime>
  Visibility: Visibility
}

type Msg =
  | LoadTodoItems
  | TodoItemsLoaded of List<Todo>
  | LoadTodoItemsFailure of Exception
  | SetNewTextDescription of string
  | SetNewDueDate of DateTime
  | SetNewDueTime of DateTime
  | ClearNewDueDate
  | ClearNewDueTime
  | AddTodo
  | AddTodoFailed
  | TodoAdded of Todo
  | DeleteTodo of TodoId
  | DeleteTodoFailure of TodoError
  | ToggleCompleted of TodoId
  | ToggleCompletedFailure of TodoError
  | SetVisibility of Visibility