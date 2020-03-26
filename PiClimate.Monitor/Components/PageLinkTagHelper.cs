// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Copyright © 2020 Maxim Yudin <stibiu@yandex.ru>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace PiClimate.Monitor.Components
{
  /// <summary>
  ///   A tag helper class that creates an HTML <c>a</c> tag link to the specified page and highlights it if the
  ///   current page route data matches.
  /// </summary>
  [HtmlTargetElement("page-link")]
  public class PageLinkTagHelper : TagHelper
  {
    /// <summary>
    ///   The URL helper service instance.
    ///   Provided via dependency injection.
    /// </summary>
    private readonly IUrlHelperFactory _urlHelperFactory;

    /// <summary>
    ///   Gets or sets the current page view context.
    ///   Initially set by the dependency injection.
    /// </summary>
    [ViewContext]
    public ViewContext? ViewContext { get; set; }

    /// <summary>
    ///   Gets or sets the page name where the created link will redirect.
    /// </summary>
    public string Page { get; set; } = "Index";

    /// <summary>
    ///   Gets or sets the page handler name where the created link will redirect.
    ///   Can be ignored if no page handler is needed.
    /// </summary>
    public string Handler { get; set; } = "";

    /// <summary>
    ///   Gets or sets the CSS class to be applied to the created HTML <c>a</c> tag if the current page route data
    ///   matches the provided <see cref="Page" />, <see cref="Handler" />, and <see cref="QueryParameters" />
    ///   properties.
    ///   Defaults to <c>active</c>.
    /// </summary>
    public string ActiveClass { get; set; } = "active";

    /// <summary>
    ///   Gets or sets the dictionary of query parameters to be added to the created link.
    /// </summary>
    public IDictionary<string, object> QueryParameters { get; set; } = new Dictionary<string, object>();

    /// <summary>
    ///   Initializes a new page link tag helper instance.
    /// </summary>
    /// <param name="urlHelperFactory">
    ///   The URL helper service instance.
    ///   Provided via dependency injection.
    /// </param>
    public PageLinkTagHelper(IUrlHelperFactory urlHelperFactory)
    {
      _urlHelperFactory = urlHelperFactory;
    }

    /// <summary>
    ///   Processes the tag helper data and outputs the HTML <c>a</c> tag containing the link to the page specified
    ///   by the provided <see cref="Page" />, <see cref="Handler" />, and <see cref="QueryParameters" /> properties.
    /// </summary>
    /// <param name="context">
    ///   The captured tag helper context data object.
    /// </param>
    /// <param name="output">
    ///   The tag helper output data provider object.
    /// </param>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
      if (ViewContext == null)
        ViewContext = new ViewContext();
      var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

      output.TagName = "a";
      output.Attributes.SetAttribute("href", urlHelper.Page(Page, Handler, QueryParameters));

      if (PageRouteMatches())
        output.AddClass(ActiveClass, HtmlEncoder.Default);
    }

    /// <summary>
    ///   Determines whether the current page route matches the page link parameters.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if the current page name and page handler match the values of the <see cref="Page" /> and
    ///   <see cref="Handler" /> properties, and the request query parameters contain all the matching keys and values
    ///   in the <see cref="QueryParameters" /> dictionary.
    ///   Otherwise returns <c>false</c>.
    /// </returns>
    private bool PageRouteMatches()
    {
      if (ViewContext == null)
        return false;

      if (!string.IsNullOrEmpty(Page) && ViewContext.RouteData.Values["page"]?.ToString()
        ?.Equals($"/{Page.Trim('/')}", StringComparison.InvariantCultureIgnoreCase) != true)
        return false;

      if (!string.IsNullOrEmpty(Handler) && ViewContext.RouteData.Values["handler"]?.ToString()
        ?.Equals(Handler, StringComparison.InvariantCultureIgnoreCase) != true)
        return false;

      if (QueryParameters.Any(pair => !ViewContext.HttpContext.Request.Query.ContainsKey(pair.Key) ||
        ViewContext.HttpContext.Request.Query[pair.Key] != pair.Value.ToString()))
        return false;

      return true;
    }
  }
}
