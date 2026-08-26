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

            foreach (var param in userDefinedParams ?? Enumerable.Empty<UserDefinedParameter>())
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

			var additionalFilter = ExtendedFilteringProperties.ValidateForContext(Context, AdditionalFilter);
			if(!additionalFilter.IsValid)
				validationErrors = validationErrors.Append(new MenuItemErrorModel { MenuItemName = Name, ErrorMessages = [additionalFilter.Error] });

            return !validationErrors.Any();
        }
	}
}
