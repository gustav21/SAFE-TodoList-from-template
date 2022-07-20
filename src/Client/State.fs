module Todo.State

open Todo.Types
open Elmish

let initialState() =
    let initState = {
        TodoItems = []
        NewTodoDescription = None
        NewTodoDueDate = None
        Visibility = All
    }

    initState, Cmd.ofMsg LoadTodoItems


let update (msg: Msg) (prevState: State) =
    match msg with
    | SetNewTextDescription text ->
        let nextState = { prevState with NewTodoDescription = Some text }
        nextState, Cmd.none

    | SetNewDueDate date ->
        let newDate =
            match prevState.NewTodoDueDate with
            | Some prevDate -> date + prevDate.TimeOfDay
            | None -> date
        let nextState = { prevState with NewTodoDueDate = Some newDate }
        nextState, Cmd.none

    | SetNewDueTime date ->
        let newDate =
            match prevState.NewTodoDueDate with
            | Some prevDate -> prevDate.Date + date.TimeOfDay
            | None -> date
        let nextState = { prevState with NewTodoDueDate = Some newDate }
        nextState, Cmd.none

    | ClearNewDueDate ->
        let nextState = { prevState with NewTodoDueDate = None }
        nextState, Cmd.none

    | ClearNewDueTime ->
        let newDate =
            match prevState.NewTodoDueDate with
            | Some prevDate -> Some prevDate.Date
            | None -> None
        let nextState = { prevState with NewTodoDueDate = newDate }
        nextState, Cmd.none

    | LoadTodoItems ->
        prevState, Server.loadAllTodos()

    | AddTodo ->
        match prevState.NewTodoDescription with
        | Some "" | None -> prevState, Cmd.none
        | Some text -> prevState, Server.addTodo text prevState.NewTodoDueDate

    | TodoAdded todoItem ->
        let nextTodoItems = List.append prevState.TodoItems [todoItem]
        let nextState = { prevState with TodoItems = nextTodoItems; NewTodoDescription = None; NewTodoDueDate = None }
        nextState, Cmd.none

    | TodoItemsLoaded items ->
        let nextState = { prevState with TodoItems = items }
        nextState, Cmd.none

    | ToggleCompleted id ->
        prevState, Server.toggleCompleted id

    | DeleteTodo id ->
        prevState, Server.deleteTodo id

    | SetVisibility visibility ->
        let nextState = { prevState with Visibility = visibility }
        nextState, Cmd.none

    | _ -> prevState, Cmd.none