﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ExtendableEnums.Microsoft.AspNetCore;

/// <summary>
/// <see cref="ITagHelper"/> tag helper targeting select tag elements with extendable-enum-for attributes.
/// </summary>
[HtmlTargetElement("select", Attributes = forAttributeName)]
public class ExtendableEnumSelectTagHelper : SelectTagHelper
{
    private const string forAttributeName = "extendable-enum-for";
    private const string isOrderedByValueName = "extendable-enum-order-by-value";

    /// <summary>
    /// Initializes a new instance of the <see cref="ExtendableEnumSelectTagHelper"/> class.
    /// </summary>
    /// <param name="generator">The current <see cref="IHtmlGenerator"/>.</param>
    public ExtendableEnumSelectTagHelper(IHtmlGenerator generator)
        : base(generator)
    {
    }

    /// <summary>
    /// Gets or sets an expression to be evaluated against the current model that points to the ExtendableEnum property.
    /// </summary>
    [HtmlAttributeName(forAttributeName)]
    public ModelExpression ExtendableFor
    {
        get => For;
        set => For = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether or not the list of options should be sorted by Value.  If not, the items will be sorted by DisplayName.
    /// </summary>
    [HtmlAttributeName(isOrderedByValueName)]
    public bool IsOrderedByValue
    {
        get; set;
    }

    /// <inheritdoc/>
    public override void Init(TagHelperContext context)
    {
        var modelType = For.ModelExplorer.ModelType;
        if (modelType.IsExtendableEnum())
        {
            var selectItems = new List<SelectListItem>();

            var getAllMethod = modelType.GetMethod("GetAll", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            var result = getAllMethod.Invoke(null, null);

            var items = result as IEnumerable<object>;
            var valueProperty = items.First().GetType().GetProperty("Value");
            var displayNameProperty = items.First().GetType().GetProperty("DisplayName");

            foreach (var item in items)
            {
                var value = valueProperty.GetValue(item, null);
                var displayName = displayNameProperty.GetValue(item, null);
                var selectItem = new SelectListItem($"{displayName}", $"{value}");
                selectItems.Add(selectItem);
            }

            if (IsOrderedByValue)
            {
                selectItems = selectItems.OrderBy(x => x.Value).ToList();
            }
            else
            {
                selectItems = selectItems.OrderBy(x => x.Text).ToList();
            }

            Items = selectItems;

            // set the current value as selected.
            if (For?.Model != null)
            {
                var modelValue = valueProperty.GetValue(For.Model, null);
                var forName = For.Name;
                if (!string.IsNullOrEmpty(ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix))
                {
                    forName = $"{ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix}.{forName}";
                }

                ViewContext.ModelState.SetModelValue(forName, modelValue, modelValue.ToString());
            }
        }

        base.Init(context);
    }
}