namespace NewTunes

    open System.Collections.ObjectModel
    open Caliburn.Micro
    open System
    open System.IO
    open Newtonsoft.Json
    open System.Windows

    type MainPageViewModel(repository : iTunesRepository) as x =
        inherit Screen()       
        let testArtistIds = "273179909,277228393,106043911,160711176"
        let artists = new ObservableCollection<ItemViewModel>()
        let artistCollections = new ObservableCollection<ItemViewModel>()
                        
        let parseArtistCollection (json:string) =
            Deployment.Current.Dispatcher.BeginInvoke(fun _ -> 
                artistCollections.Clear()
                artists.Clear()
                let jsonResults = JsonConvert.DeserializeObject<iTunesJsonResults> json 
                jsonResults.JsonResults 
                |> Seq.filter(fun row -> row.CollectionName.Trim() <> "")
                |> Seq.iter(fun artistCollection -> artistCollections.Add(artistCollection)) 
                artistCollections |> Seq.distinctBy(fun a -> a.ArtistName)
                |> Seq.iter(fun artist -> artists.Add(artist))
                x.NotifyOfPropertyChanges()) |> ignore

        let jsonProcessingAgent = MailboxProcessor.Start(fun inbox ->
                                                            async { while true do
                                                                    let! json = inbox.Receive()
                                                                    parseArtistCollection json })

        let retrieveArtistCollections artistIds =
            repository.GetAllByArtistIds jsonProcessingAgent artistIds

        do retrieveArtistCollections testArtistIds

        member internal x.NotifyOfPropertyChanges() =
            base.NotifyOfPropertyChange("AllArtists")

        member x.BuildArtists artistIds =
            retrieveArtistCollections testArtistIds
            
        member x.FindArtists searchTerm =
            repository.GetAllBySearch jsonProcessingAgent searchTerm

        member x.AllArtists = artists
                