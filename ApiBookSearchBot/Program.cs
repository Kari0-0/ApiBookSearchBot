
using ApiBookSearchBot;
using ApiBookSearchBot.Models;
using ApiBookSearchBot.Reposytory;
using Newtonsoft.Json;


string lastUri = "https://www.googleapis.com/books/v1/volumes?q=text:ukrain";

Reposytory reposytory = new Reposytory();

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();  

app.MapGet("/", () => "Api Book Search Bot");   
app.MapGet($"/{Const.BOOK_NAME_SEARCH}/{{{Const.BOOK_NAME}}}",  
           async (string bookName) =>   
{
    lastUri = $"https://www.googleapis.com/books/v1/volumes?q=intitle:{bookName}";
    Uri uri = new Uri(lastUri); 

    string json = await GetStringResponseByUri(uri);

    FullGoogleModels gogmodels = JsonConvert.DeserializeObject<FullGoogleModels>(json);
    if (gogmodels.items == null)
    {
        return JsonConvert.SerializeObject(new BookModel() { books = new List<Book>() { new Book() { title = "No have((" } } });
    }
    if (gogmodels.items == null)
    {
    return JsonConvert.SerializeObject(new BookModel() { books = new List<Book>() { new Book() { title = "No have(("} } });
    }
    var bookModel = BookModel.FullGoogleModelToBookModel(gogmodels);
    return JsonConvert.SerializeObject(bookModel);
});

app.MapGet($"/{Const.BOOK_ISBN_SEARCH}/{{{Const.ISBN}}}", 
           async (string isbn) =>
{
    lastUri = $"https://www.googleapis.com/books/v1/volumes?q=isbn:{isbn}";
    Uri uri = new Uri(lastUri); 

    try    
    {
        string json = await GetStringResponseByUri(uri);

        FullGoogleModels gogmodels = JsonConvert.DeserializeObject<FullGoogleModels>(json);
        if (gogmodels.items == null)
        {
            return JsonConvert.SerializeObject(new BookModel() { books = new List<Book>() { new Book() { title = "No have((" } } });
        }
        var bookModel = BookModel.FullGoogleModelToBookModel(gogmodels);
        
        return JsonConvert.SerializeObject(bookModel);
    }
    catch(Exception ex)
    {
        app.Logger.LogError(ex.Message);
        return JsonConvert.SerializeObject(new BookModel());
    }
});
app.MapGet($"/{Const.BOOK_AUTHOR_SEARCH}/{{{Const.AUTHOR}}}",   
           async (string author) =>
{
    lastUri = $"https://www.googleapis.com/books/v1/volumes?q=inauthor:{author}";

    Uri uri = new Uri(lastUri); 

    string json = await GetStringResponseByUri(uri);

    FullGoogleModels gogmodels = JsonConvert.DeserializeObject<FullGoogleModels>(json);
    if (gogmodels.items == null)
    {
        return JsonConvert.SerializeObject(new BookModel() { books = new List<Book>() { new Book() { title = "No have((" } } });
    }
    var bookModel = BookModel.FullGoogleModelToBookModel(gogmodels);

    return JsonConvert.SerializeObject(bookModel);
});
app.MapGet($"/{Const.BOOK_BOOKCOVER_SEARCH}/{{{Const.ISBN}}}", 
           async (string isbn) =>
{
    Uri uri = new Uri($"https://covers.openlibrary.org/b/isbn/{isbn}-M.jpg");

    return Results.Json(uri);
});
app.MapGet($"/{Const.BOOK_RANDOMBOOK}",    
           async () =>
{
    Uri uri = new Uri(lastUri);

    string json = await GetStringResponseByUri(uri);

    FullGoogleModels gogmodels = JsonConvert.DeserializeObject<FullGoogleModels>(json);
    if (gogmodels.items == null)
    {
        return JsonConvert.SerializeObject(new Book() { title = "No have((" });
    }
    var bookModel = BookModel.FullGoogleModelToBookModel(gogmodels);

    Random rnd = new Random();

    var book = bookModel.books[rnd.Next(0, bookModel.books.Count)];

    return JsonConvert.SerializeObject(book);
});
app.MapGet($"/{Const.GET_ALL_BOOKS}/{{{Const.TG_ID}}}",     
           async (string tgid) =>
{
    return Results.Json(reposytory.GetBooks(tgid));
});

app.MapPost($"/{Const.ADD_USER}",  
            async (context) =>     
{
    try   
    {
        var user = new TelegramUser()
        {
            TgId = context.Request.Query[Const.TG_ID],
            Name = context.Request.Query[Const.NAME],
        };

        reposytory.AddUser(user);
    }
    catch(Exception ex) 
    {
    
        app.Logger.LogError(ex.Message);

        await context.Response.WriteAsJsonAsync((new StatusRequest(StatusRequest.FAIL)));
    }
    await context.Response.WriteAsJsonAsync((new StatusRequest(StatusRequest.OK)));
});
app.MapPost($"/{Const.ADD_BOOK}",   
            async (context) =>
{
    try     
    {
        UserBook book = new UserBook()
        {
            Isbn = context.Request.Query[Const.ISBN]
        };

        reposytory.AddBook(book, context.Request.Query[Const.TG_ID]);
    }
    catch (Exception ex) 
    {
        app.Logger.LogError(ex.Message);
        await context.Response.WriteAsJsonAsync((new StatusRequest(StatusRequest.FAIL)));
    }
    await context.Response.WriteAsJsonAsync((new StatusRequest(StatusRequest.OK)));
});

app.MapGet("/users", () => reposytory.GetUsers());  
app.MapGet("/books", () => reposytory.GetBooks()); 

app.MapDelete($"/{Const.DELETE_BOOK}",  
              async (context) =>
{
    try    
    {
        var book = new UserBook()
        {
            Isbn = context.Request.Query[Const.ISBN]
        };

        reposytory.DeleteBook(book, context.Request.Query[Const.TG_ID]);
    }
    catch (Exception ex) 
    {
        
        app.Logger.LogError(ex.Message);

        
        await context.Response.WriteAsJsonAsync((new StatusRequest(StatusRequest.FAIL)));
    }
    
    await context.Response.WriteAsJsonAsync((new StatusRequest(StatusRequest.OK)));
});
app.MapDelete($"/{Const.DELETE_ALL_BOOKS}", 
              async (context) =>
{
    try 
    {
        reposytory.DeleteAllBook(context.Request.Query[Const.TG_ID]);
    }
    catch (Exception ex) 
    {
        app.Logger.LogError(ex.Message);

        await context.Response.WriteAsJsonAsync((new StatusRequest(StatusRequest.FAIL)));
    }
    await context.Response.WriteAsJsonAsync(new StatusRequest(StatusRequest.OK));
});

async Task<string> GetStringResponseByUri(Uri uri)
{
    string json;
    using (HttpClient httpClient = new HttpClient())
    {
        using (var response = await httpClient.GetAsync(uri))
        {
            json = await response.Content.ReadAsStringAsync();
        }
    }
    return json;
}
app.Run();
