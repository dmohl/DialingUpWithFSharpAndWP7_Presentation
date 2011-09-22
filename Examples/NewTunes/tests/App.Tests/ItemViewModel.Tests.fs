namespace NewTunes.Tests

open FsUnit
open NUnit.Framework
open NewTunes

[<TestFixture>]
type ``When creating a new ItemViewModel``() =
    [<DefaultValue>] val mutable result : ItemViewModel 
    [<TestFixtureSetUp>] 
    member test.``Because Of``() =
        test.result <- new ItemViewModel()
        test.result.ArtistId <- "1"
        test.result.ArtistName <- "Some Artist"
        test.result.CollectionName <- "Some Collection"
        test.result.ArtworkUrl100 <- "http://test.com"
        test.result.ReleaseDate <- "01/01/2011"
    [<Test>] 
    member test.``Should have a ArtistId of 1``() =
        test.result.ArtistId |> should equal "1"
    [<Test>] 
    member test.``Should have a ArtistName of Some Artist``() =
        test.result.ArtistName |> should equal "Some Artist"
    [<Test>] 
    member test.``Should have a CollectionName of Some Collection``() =
        test.result.CollectionName |> should equal "Some Collection"
    [<Test>] 
    member test.``Should have a ArtworkUrl100 of http://test.com``() =
        test.result.ArtworkUrl100 |> should equal "http://test.com"
    [<Test>] 
    member test.``Should have a ReleaseDate of 01/01/2011``() =
        test.result.ReleaseDate |> should equal "01/01/2011"