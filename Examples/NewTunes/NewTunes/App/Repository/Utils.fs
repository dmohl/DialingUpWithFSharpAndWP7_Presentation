﻿[<AutoOpen>]
module Utils
open System
open System.Windows.Controls

// From http://tomasp.net/blog/reactive-talk.aspx

let (?) (this : Control) (prop : string) : 'T =
  this.FindName(prop) :?> 'T

let synchronize f = 
  let ctx = System.Threading.SynchronizationContext.Current 
  f (fun g arg ->
    let nctx = System.Threading.SynchronizationContext.Current 
    if ctx <> null && ctx <> nctx then ctx.Post((fun _ -> g(arg)), null)
    else g(arg) )

module Observable = 
  let guard f (e:IObservable<'Args>) =  
    { new IObservable<'Args> with  
        member x.Subscribe(observer) =  
          let rm = e.Subscribe(observer) in f(); rm } 

type Microsoft.FSharp.Control.Async with 
  static member GuardedAwaitObservable (ev1:IObservable<'a>) gfunc =
    synchronize (fun f ->
      Async.FromContinuations((fun (cont,econt,ccont) -> 
        let rec callback = (fun value ->
          remover.Dispose()
          f cont value )
        and remover : IDisposable  = ev1.Subscribe(callback) 
        gfunc() )))

  static member AwaitObservable(ev1:IObservable<'a>) =
    synchronize (fun f ->
      Async.FromContinuations((fun (cont,econt,ccont) -> 
        let rec callback = (fun value ->
          remover.Dispose()
          f cont value )
        and remover : IDisposable  = ev1.Subscribe(callback) 
        () )))
  
  static member AwaitObservable(ev1:IObservable<'a>, ev2:IObservable<'b>) = 
    synchronize (fun f ->
      Async.FromContinuations((fun (cont,econt,ccont) -> 
        let rec callback1 = (fun value ->
          remover1.Dispose()
          remover2.Dispose()
          f cont (Choice1Of2(value)) )
        and callback2 = (fun value ->
          remover1.Dispose()
          remover2.Dispose()
          f cont (Choice2Of2(value)) )
        and remover1 : IDisposable  = ev1.Subscribe(callback1) 
        and remover2 : IDisposable  = ev2.Subscribe(callback2) 
        () )))

  static member AwaitObservable(ev1:IObservable<'a>, ev2:IObservable<'b>, ev3:IObservable<'c>) = 
    synchronize (fun f ->
      Async.FromContinuations((fun (cont,econt,ccont) -> 
        let rec callback1 = (fun value ->
          remover1.Dispose()
          remover2.Dispose()
          remover3.Dispose()
          f cont (Choice1Of3(value)) )
        and callback2 = (fun value ->
          remover1.Dispose()
          remover2.Dispose()
          remover3.Dispose()
          f cont (Choice2Of3(value)) )
        and callback3 = (fun value ->
          remover1.Dispose()
          remover2.Dispose()
          remover3.Dispose()
          f cont (Choice3Of3(value)) )
        and remover1 : IDisposable  = ev1.Subscribe(callback1) 
        and remover2 : IDisposable  = ev2.Subscribe(callback2) 
        and remover3 : IDisposable  = ev3.Subscribe(callback3) 
        () )))
