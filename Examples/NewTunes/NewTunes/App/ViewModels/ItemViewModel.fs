namespace NewTunes

open System.Runtime.Serialization
open System.Runtime.Serialization.Json
open Caliburn.Micro

[<DataContract()>]
type ItemViewModel() =
    let mutable artistId = ""
    let mutable artistName = ""
    let mutable collectionName = ""
    let mutable artworkUrl100 = ""
    let mutable releaseDate = ""
    [<DataMember(Name="artistId")>] member x.ArtistId with get() = artistId and set(v) = artistId <- v
    [<DataMember(Name="artistName")>] member x.ArtistName with get() = artistName and set(v) = artistName <- v
    [<DataMember(Name="collectionName")>] member x.CollectionName with get() = collectionName and set(v) = collectionName <- v
    [<DataMember(Name="artworkUrl100")>] member x.ArtworkUrl100 with get() = artworkUrl100 and set(v) = artworkUrl100 <- v
    [<DataMember(Name="releaseDate")>] member x.ReleaseDate with get() = releaseDate and set(v) = releaseDate <- v
    
[<DataContract()>]
type iTunesJsonResults() =
    let mutable jsonResults:ItemViewModel[] = [||]
    [<DataMember(Name="results")>] member x.JsonResults with get() = jsonResults and set(v) = jsonResults <- v
