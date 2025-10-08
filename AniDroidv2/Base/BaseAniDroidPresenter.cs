using System.Collections.Generic;
using System.Threading.Tasks;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.Utils.Interfaces;
using AniDroidv2.Utils.Logging;

namespace AniDroidv2.Base
{
    public abstract class BaseAniDroidv2Presenter
    {
        public IAniDroidv2View View { get; set; }
        public IAniDroidv2Settings AniDroidv2Settings { get; }
        public IAniDroidv2Logger Logger { get; }

        protected IAniListService AniListService { get; }

        protected BaseAniDroidv2Presenter(IAniListService service, IAniDroidv2Settings settings,
            IAniDroidv2Logger logger)
        {
            AniDroidv2Settings = settings;
            Logger = logger;
            AniListService = service;
        }

        //Any initial calls to the view or api calls should go here
        //Do not put initialization in the constructor because Android may need to recreate the presenter from a saved state
        public abstract Task Init();

        public abstract Task BaseInit(IAniDroidv2View view);

        //These methods are to allow the presenter to be restored properly on Android when the View is killed by the system
        public virtual Task RestoreState(IList<string> savedState)
        {
            return Task.CompletedTask;
        }

        public virtual IList<string> SaveState()
        {
            return new List<string>();
        }
    }

    public abstract class BaseAniDroidv2Presenter<T> : BaseAniDroidv2Presenter where T : IAniDroidv2View
    {
        protected BaseAniDroidv2Presenter(IAniListService service, IAniDroidv2Settings settings,
            IAniDroidv2Logger logger) : base(service, settings, logger)
        {
        }

        public new T View { get; set; }

        public sealed override Task BaseInit(IAniDroidv2View view)
        {
            View = (T)view;

            Init();

            return Task.CompletedTask;
        }
    }
}