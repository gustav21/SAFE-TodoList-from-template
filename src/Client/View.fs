module Todo.View

open System
open System.Globalization
open Shared
open Todo.Types
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props

let divider =  span [Style [ MarginLeft 5; MarginRight 5 ]]  [ ]

let renderTodo (item: Todo) dispatch =
    let toggleText = if item.Completed then "Actually, Not Yet!" else "Complete"
    let dispatchToggle = OnClick (fun _ -> dispatch (ToggleCompleted item.Id))
    let dispatchDelete = OnClick (fun _ -> dispatch (DeleteTodo item.Id))

    let todoStyle =
      match item.Completed with
      | true ->  Style [ Color "red"; FontSize 19; Padding 5; TextDecoration "line-through"]
      | false ->  Style [ Color "green"; FontSize 19; Padding 5 ]

    let dateToString (date:DateTime option) =
        match date with
        | Some date ->
            if date.Date = date then date.ToShortDateString()
            else date.ToString(CultureInfo.InvariantCulture)
        | None -> ""

    div
      [ ]
      [ p [ todoStyle ]
          [ yield span [] [ str item.Description ]
            if item.DueDate.IsSome then
                yield span [ Style [ FontSize 14; PaddingLeft 5 ] ] [ str (dateToString item.DueDate) ] ]
        button [ ClassName "button is-info"; dispatchToggle ] [ str toggleText ]
        divider
        button [ ClassName "button is-danger"; dispatchDelete ] [ str "Delete" ] ]


let addTodo (state: State) dispatch =
  let textValue = defaultArg state.NewTodoDescription ""
  div
    [ ClassName "field has-addons"; Style [Padding 5; Width 400] ]
    [ div
        [ ClassName "control is-large" ]
        [ input [ ClassName "input is-large"
                  Placeholder "Add Todo"
                  DefaultValue textValue
                  Value textValue
                  OnChange (fun ev -> dispatch (SetNewTextDescription (!!ev.target?value)))] ]
      div
        [ ClassName "control is-large" ]
        [ button [ ClassName "button is-primary is-large"; OnClick (fun _ -> dispatch AddTodo) ] [ str "Add Todo" ] ] ]

let visibilityCombo dispatch =
    let parseVisibility value =
        match Visibility.parse value with
        | Some visibility -> visibility
        | None -> Visibility.All

    div
        [ ClassName "select" ]
        [ select
            [ OnChange (fun ev -> dispatch (SetVisibility (parseVisibility !!ev.target?value))) ]
            [ option [ Value Visibility.All ] [ str "All" ]
              option [ Value Visibility.Completed ] [ str "Completed" ]
              option [ Value Visibility.NotCompleted ] [ str "Not completed" ] ] ]

let render  (state: State) dispatch =
    let sortedTodos =
      state.TodoItems
      |> List.filter (fun todo -> match state.Visibility with
                                  | Visibility.All -> true
                                  | Completed -> todo.Completed
                                  | NotCompleted -> todo.Completed = false)
      |> List.sortBy (fun todo -> todo.DateAdded)
      |> List.map (fun todo -> renderTodo todo dispatch)

    let controls =
        [visibilityCombo]
        |> List.map (fun renderControl -> renderControl dispatch)

    div
     [ Style [ Padding 20 ] ]
     [ yield h1 [ Style [ FontSize 24 ] ] [ str "SAFE Todo-List" ]
       yield hr [ ]
       yield addTodo state dispatch
       yield div [ Style [ Padding 5 ] ] controls
       yield! sortedTodos ]