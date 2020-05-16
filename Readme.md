# Hateoas

An easy to use framework to wrap the the objects and add Hyper Media to them.

Having a general formula for generating all HATEOAS links is a bad practice since the links should be attached depending on the situation. Vlerx.Hateoas is a library that lets you attach the links easily and customize the language around your link generation.

## How it works
Install [this](https://www.nuget.org/packages/Vlerx.Hateoas/) nuget package.

Look at these three actions in the user controller.
```cs
public class UserController : ControllerBase
{
     [HttpPost(Name = nameof(Register))]
     public IActionResult Register(RegisterUserRequestDto dto)
     {
        ...
        var result = addLinks.ForNoContent()
                             .ToSelf(dto.UserName)
                             .ToListUsers()
                             .Result;
        return Accepted(result);
     }
     
     [HttpGet("{userName}", Name = nameof(FindByUserName))]
     public IActionResult FindByUserName(string userName)
     {
        ...
        var result = addLinks.ForContent(viewModel)
                                 .ToSelf(userName)
                                 .ToListUsers()
                                 .Result;
        return Ok(result);
     }
      
      [HttpGet("", Name = nameof(ListUsers))]
      public IActionResult ListUsers([FromQuery]PageRequest pagedRequest)
      {
          var users = userQueries.ListUsers(pagedRequest ?? new PageRequest { PageNumber = 1, PageSize = 10 });
          Response.Headers.AddPagination(users.Counter);
          var result = addLinks.ForPageContent(users, pagedRequest)
                               .AddLinksToItems(i => addLinks.ForContent(i)
                                                             .ToSelf(i.UserName)
                                                             .ToListUsers().Result);
          return Ok(result);
      }
     public UserController(UserLinksBuilder userLinksBuilder)
     {
        this.addLinks = userLinksBuilder;
     }
     readonly UserLinksBuilder addLinks;
}
````
The UserLinksBuilder class your custom API for building the links:
```cs
public class UserLinksBuilder
{
    readonly IUrlHelper url;
    public UserLinksBuilder(IUrlHelper url)
    => this.url = url;
    public UserItemLinksBuilder ForContent<T>(T source)
    => new UserItemLinksBuilder(HateoasLinks.WithContent(source), url);
    public UserItemLinksBuilder ForNoContent()
    => new UserItemLinksBuilder(HateoasLinks.WithNoContent, url);

    public UserPageLinkBuilder<T> ForPageContent<T>(PagedViewModel<T> pagedVm, PageRequest currentPageRequest)
    => new UserPageLinkBuilder<T>(url, pagedVm, currentPageRequest);
```
It uses this tiny nested class to build link to single item results:
```cs
    public class UserItemLinksBuilder : LinksBuilder<UserItemLinksBuilder>
    {
        internal UserItemLinksBuilder(HateoasLinks result, IUrlHelper url) : base(result, url) { }
        public UserItemLinksBuilder ToSelf(string userName)
        => AddLink(href: url.Link(nameof(UserController.FindByUserName), new { userName }),
                   rel: "self",
                   type: HttpMethods.Get);

        public UserItemLinksBuilder ToListUsers()
        => AddLink(href: url.Link(nameof(UserController.ListUsers), new PageRequest { PageNumber = 1, PageSize = 10 }),
                   rel: "list",
                   type: HttpMethods.Get);
    }
```
And this tiny nested class to build link to collection results:
```cs
    public class UserPageLinkBuilder<T> : PageLinkBuilder<T>
    {
        internal UserPageLinkBuilder(IUrlHelper url, PagedViewModel<T> pagedVm, PageRequest currentPageRequest) : base(url, pagedVm, currentPageRequest) { }
        public LinkResult<IEnumerable<HateoasLinks>> AddLinksToItems(Func<T, HateoasLinks> howToAddLinkToEachItem)
        => AddPaginationLinks(new PageHrefs(
                current: url.Link(nameof(UserController.ListUsers), currentPageRequest),
                next: url.Link(nameof(UserController.ListUsers), new PageRequest { PageNumber = currentPageRequest.PageNumber + 1, PageSize = currentPageRequest.PageSize }),
                previous: url.Link(nameof(UserController.ListUsers), new PageRequest { PageNumber = currentPageRequest.PageNumber - 1, PageSize = currentPageRequest.PageSize }))
            , howToAddLinkToEachItem);
    }
}
```

## Why a separate links builder?
Don't Repeate Yourself is pretty relevant for building the links. Adding the creation of the links to the responsiblities of a controller usually ends up a bloated messy controller. The controller want's to attach different combinations of different links in different situations easily. The builder we saw on top lets the controller combine the links for different results in a readable language. It is also a single source of change for links.
