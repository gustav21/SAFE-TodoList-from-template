module Client.Tests

open Fable.Mocha

open Shared
open Todo
open Todo.Types

let client = testList "Client" [
    testCase "Added todo" <| fun _ ->
        let newTodo = { defaultTodo() with Description = "new todo" }
        let model, _ = State.initialState ()

        let model, _ = State.update (TodoAdded newTodo) model

        Expect.equal 1 model.TodoItems.Length "There should be 1 todo"
        Expect.equal newTodo model.TodoItems.[0] "Todo should equal new todo"
]

let all =
    testList "All"
        [
            client
        ]

[<EntryPoint>]
let main _ = Mocha.runTests all