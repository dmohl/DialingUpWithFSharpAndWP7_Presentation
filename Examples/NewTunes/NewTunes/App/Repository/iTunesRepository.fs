namespace NewTunes

open System
open System.IO
open System.Net

[<AutoOpen>]
module HttpWebRequestHelper =
    type System.Net.HttpWebRequest with 
        member x.GetResponseAsync() = 
            Async.FromBeginEnd(x.BeginGetResponse, x.EndGetResponse) 

type iTunesRepository() =
    let urlToSearchByArtistIds = "http://ax.phobos.apple.com.edgesuite.net/WebObjects/MZStoreServices.woa/wa/wsLookup?id={0}&entity=album&sort=recent"
    let urlToSearchBySearchTerm = "http://ax.phobos.apple.com.edgesuite.net/WebObjects/MZStoreServices.woa/wa/wsSearch?term=%s&media=music&entity=album&attribute=artistTerm&limit=10"
        
    let BuildITunesUrl urlTemplate searchTerm =
        String.Format(urlTemplate, [|searchTerm|])
    
    member x.GetAllBySearch jsonProcessingAgent criteria = 
        x.GetAll jsonProcessingAgent urlToSearchBySearchTerm criteria

    member x.GetAllByArtistIds jsonProcessingAgent criteria =
        x.GetAll jsonProcessingAgent urlToSearchByArtistIds criteria

    member x.GetAll (jsonProcessingAgent:MailboxProcessor<_>) urlTemplate criteria =
        async {
            let url = BuildITunesUrl urlTemplate (Uri.EscapeDataString criteria)  
            let webClient = new WebClient()
            let! result = Async.GuardedAwaitObservable webClient.DownloadStringCompleted 
                            (fun () -> webClient.DownloadStringAsync(new Uri(url)))             
            jsonProcessingAgent.Post(result.Result) }
        |> Async.StartImmediate 
