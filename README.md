# GameCtor.XPlatLibs

## FirebaseAuth.DotNet Samples

```csharp
authService.SignInSuccessful
    .SelectMany(authToken => AuthenticateWithFirebase(authToken))
    .SelectMany(_ => NavigationService.PushPage(new MainViewModel()))
    .Subscribe();
    
private IObservable<Unit> AuthenticateWithFirebase(string authToken)
{
    IObservable<Unit> result = null;
    if(_provider == "google")
    {
        result = _firebaseAuthService
            .SignInWithGoogle(null, authToken);
    }
    else if(_provider == "facebook")
    {
        result = _firebaseAuthService
            .SignInWithFacebook(authToken);
    }

    return result;
}
```

## FirebaseDatabase.DotNet Samples

```csharp
public class CatalogCategory : IModel
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("imageUrl")]
    public string ImageUrl { get; set; }
}

public class RepoService
{
    public IRepository<CatalogCategory> CatalogCategoryRepo
    {
        get { return new FirebaseOfflineRepo<CatalogCategory>(_firebaseClient, PATH_CATALOG_CATEGORIES); }
    }
}

var categoryRepo = RepoService.Instance.CatalogCategoryRepo;
var categories = new ObservableCollection<CatalogCategoryCellViewModel>();

categoryRepo
    .GetItems()
    .SelectMany(x => x)
    .Select(x => new CatalogCategoryCellViewModel(x))
    .Subscribe(x => categories.Add(x));
```
