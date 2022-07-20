[<RequireQualifiedAccess>]
module Todo.FlatpickrExt

open System

/// Defines whether or not the wrapping mode is active (https://flatpickr.js.org/examples/#flatpickr--external-elements)
let Wrap (cond:bool) =
    Flatpickr.custom "wrap" cond true

/// Registers an event handler for Flatpickr that is triggered when the user selects a new datetime value
/// and an event handler that is triggered when the user clears the Flatpickr's input
let OnChange (onSelect: DateTime -> unit) (onClear: unit -> unit) =
    let callback =
        unbox (fun (dates: DateTime[]) ->
            if isNull (unbox<obj> dates) || Array.isEmpty dates
            then onClear()
            else onSelect dates.[0])
    Flatpickr.custom "onChange" callback false

/// Sets the initial value for the Flatpickr component (value is optional)
let Value (date: DateTime option) =
    Flatpickr.custom "value" (match date with
                              | Some date -> box date
                              | None -> null) false