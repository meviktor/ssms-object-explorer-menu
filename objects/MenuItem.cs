using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Linq;
using SSMSObjectExplorerMenu.extendedfiltering;

namespace SSMSObjectExplorerMenu.objects
{

	public class MenuItem
	{
		private string _script = string.Empty; 

		[Category("Menu item")]
		[DisplayName("Enabled")]
		[Description("Show/hide the menu item.")]
		[DefaultValue(true)]
		public bool Enabled { get; set; } = true;

		[Category("Menu item")]
		[DisplayName("Name")]
		[Description("Text displayed on menu item.")]
		[DefaultValue("")]
		public string Name { get; set; } = string.Empty;

		[Category("Menu item")]
		[DisplayName("Script")]
		[Description("Inline tsql statements OR path to script file.")]
		[DefaultValue("")]
		[Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
		public string Script 
		{ 
			get => _script; 
			set => _script = value.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine); 
		}

		[Category("Menu item")]
		[DisplayName("Execute")]
		[Description("Automatically run the selected script or tsql statements.")]
		[DefaultValue(true)]
		public bool Execute { get; set; } = true;

		[Category("Menu item")]
		[DisplayName("Confirm")]
		[Description("Ask confirmation to continue before executing script or statement.")]
		[DefaultValue(false)]
		public bool Confirm { get; set; } = false;

		[Category("Menu item")]
		[DisplayName("Context")]
		[Description("Tree node level where to display menu item.")]
		[DefaultValue("All")]
        public string Context { get; set; } = "All";

        [Category("Menu item")]
        [DisplayName("User-defined parameters")]
        [Description("List of user-deifned parameters can be used in the T-SQL script.")]
		[Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        public BindingList<UserDefinedParameter> UserDefinedParameters { get; private set; } = [];

        [Category("Menu item")]
        [DisplayName("Additional filter")]
        [Description("Applicable for the following contexts: Server, Server/Database, Server/Database/Table, Server/Database/Table/Column. It has to be empty for other context types.")]
        [DefaultValue("")]
        public string AdditionalFilter { get; set; } = string.Empty;

        [Category("Menu item")]
        [DisplayName("Use regular identifiers in additional filter")]
        [Description(@"
            If set to true, column-, table-, schema- and database names in the additional filter are validated according the naming rules of SQL Server regular identifiers.
            Setting this flag to false causes treating these identifiers as if they were delimited (only the length limit and quoutes escaping are tested).
            Not applicable for server names. They are always validated as they were delimited identifiers.")]
        [DefaultValue(true)]
        public bool UseRegularIdentifiers { get; set; } = true;

        public MenuItem()
		{
			
		}

		public MenuItem(bool enabled, string context, string name, string script, bool execute, bool confirm, string additionalFilter = "", IEnumerable<UserDefinedParameter> userDefinedParams = null)
		{
            Enabled = enabled;
			Context = context;
			Name = name;
			Script = script;
			Execute = execute;
			Confirm = confirm;
            AdditionalFilter = additionalFilter;

            foreach (var param in userDefinedParams ?? [])
            {
                UserDefinedParameters.Add(param);
            }

			if (Confirm) {
				Execute = true;
			}
		}

		public bool TryValidate(out IEnumerable<MenuItemErrorModel> validationErrors)
		{
            validationErrors = UserDefinedParameters.Select(
                // reserved names: names coming from context + names of other user-defined parameters of the MenuItem
                p => p.TryValidate(out var paramErrors, Utils.ParametersFromContext.Concat(UserDefinedParameters.Where(pa => pa != p).Select(pa => pa.Name)))
					? null : new MenuItemErrorModel { MenuItemName = Name, ErrorMessages = paramErrors })
				.Where(e => e != null);

			var additionalFilter = ExtendedFilteringProperties.BuildFromNavigationContext(AdditionalFilter, UseRegularIdentifiers, out var filterBuildErrors);

            if (additionalFilter != null && !additionalFilter.TryValidateForContext(Context, out var contextErrors))
                validationErrors = validationErrors.Append(new MenuItemErrorModel { MenuItemName = Name, ErrorMessages = [.. contextErrors] });
            else if (additionalFilter == null)
                validationErrors = validationErrors.Append(new MenuItemErrorModel { MenuItemName = Name, ErrorMessages = [.. filterBuildErrors] });

            return !validationErrors.Any();
        }
	}
}
