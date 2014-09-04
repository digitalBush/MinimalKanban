using System;
using System.Web.Optimization;

namespace Kanban {
  public class BundleConfig {
    public static void RegisterBundles(BundleCollection bundles) {
      bundles.IgnoreList.Clear();
      AddDefaultIgnorePatterns(bundles.IgnoreList);

	  bundles.Add(
		new ScriptBundle("~/Scripts/vendor")
			.Include("~/Scripts/jquery-{version}.js")
            .Include("~/Scripts/jquery-ui-sortable.js")
            .Include("~/Scripts/jquery.signalR-{version}.js")
			.Include("~/Scripts/bootstrap.js")
			.Include("~/Scripts/knockout-{version}.js")
            .Include("~/Scripts/lodash.js")
            .Include("~/App/bindings/*.js")
            .Include("~/App/helpers/ko.object.js")
            .Include("~/Scripts/medium-editor.min.js")
		);

      bundles.Add(
        new StyleBundle("~/Content/css")
          .Include("~/Content/bootstrap.min.css")
		  .Include("~/Content/durandal.css")
          .Include("~/Content/app.css")
          .Include("~/Content/medium-editor/medium-editor.min.css")
          .Include("~/Content/medium-editor/theme/default.min.css")
        );
    }

    public static void AddDefaultIgnorePatterns(IgnoreList ignoreList) {
      if(ignoreList == null) {
        throw new ArgumentNullException("ignoreList");
      }

      ignoreList.Ignore("*.intellisense.js");
      ignoreList.Ignore("*-vsdoc.js");
      ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
      //ignoreList.Ignore("*.min.js", OptimizationMode.WhenDisabled);
      //ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
    }
  }
}