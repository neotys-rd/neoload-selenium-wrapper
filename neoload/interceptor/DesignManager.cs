using NeoLoadSelenium.neoload.config;
using Neotys.DesignAPI.Client;
using Neotys.DesignAPI.Model;

namespace NeoLoadSelenium.neoload.interceptor
{
    class DesignManager
    {
        private static string DEFAULT_USER_PATH = "UserPath";

        private string validateUserPathName(string userPathName)
        {
            if(userPathName != null && !userPathName.Equals(""))
            {
                return userPathName;
            }
            return DEFAULT_USER_PATH;
        }

        public static IDesignAPIClient newDesignAPIClientFromConfig(DesignConfiguration config)
        {
            if (config.ApiKey != null && config.ApiKey != "")
            {
                return DesignAPIClientFactory.NewClient(config.DesignAPIUrl, config.ApiKey);
            }
            return DesignAPIClientFactory.NewClient(config.DesignAPIUrl);
        }

        private IDesignAPIClient designApiClient;
        private string projectPath;
        private string userPathName;
        private bool userPathExist;
        private ParamBuilderProvider paramBuilderProvider;

        public DesignManager(DesignConfiguration config, ParamBuilderProvider paramBuilderProvider)
        {
            this.designApiClient = newDesignAPIClientFromConfig(config);
            this.projectPath = config.ProjectPath;
            this.userPathName = validateUserPathName(config.UserPathName);
            this.paramBuilderProvider = paramBuilderProvider;
        }

        public void start()
        {
            if (projectPath != null)
            {
                openProject();
            }

            this.userPathExist = containsUserPath();

            StartRecordingParamsBuilder startRecordingBuilder = paramBuilderProvider.newStartRecordingBuilder();
            if (!userPathExist)
            {
                startRecordingBuilder.virtualUser(userPathName);
            }
            else
            {
                startRecordingBuilder.virtualUser(userPathName + "_recording");
            }

            designApiClient.StartRecording(startRecordingBuilder.Build());
        }

        private bool containsUserPath()
        {
            return designApiClient.ContainsUserPath(new ContainsUserPathParamsBuilder().name(userPathName).Build());
        }

        private void openProject()
        {
            bool isOpen = designApiClient.IsProjectOpen(new IsProjectOpenParamsBuilder().filePath(projectPath).Build());
            if (!isOpen)
            {
                designApiClient.CloseProject(paramBuilderProvider.newCloseProjectParamsBuilder().Build());
                designApiClient.OpenProject(new OpenProjectParamsBuilder().filePath(projectPath).Build());
            }
        }

        public void stop()
        {
            StopRecordingParamsBuilder stopRecordParams = paramBuilderProvider.newStopRecordingBuilder();
            if (userPathExist)
            {
                stopRecordParams.updateParams(paramBuilderProvider.newUpdateUserPathParamsBuilder().name(userPathName).Build());
            }
            designApiClient.StopRecording(stopRecordParams.Build());

            designApiClient.SaveProject();
        }

        public void startTransaction(string name)
        {
            designApiClient.SetContainer(new SetContainerParams(name));
        }

    }
}
