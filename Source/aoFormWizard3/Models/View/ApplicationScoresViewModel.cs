using Contensive.Addon.aoFormWizard3.Models.Db;
using Contensive.BaseClasses;
using Contensive.DesignBlockBase.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contensive.Addon.aoFormWizard3.Models.View {
    public class ApplicationScoresViewModel : DesignBlockViewBaseModel {
        
        public class ApplicationScoresTableRow {
            public string scorerFirstName { get; set; }
            public string scorerMiddleInitial { get; set; }
            public string scorerLastName { get; set; }
            public string scorerEmail { get; set; }
            public string dateSubmitted { get; set; }
            public string score { get; set; }
            public string cumulativeScore { get; set; }
            public string numberOfScoresSubmitted { get; set; }
        }
        public static ApplicationScoresViewModel create(CPBaseClass cp, FormWidgetsModel settings) {
            try {
                var viewModel = new ApplicationScoresViewModel();

                return viewModel;
            }
            catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }

    }
}
