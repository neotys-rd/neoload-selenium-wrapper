using Neotys.DesignAPI.Model;

namespace NeoLoadSelenium.neoload.config
{
    public class ParamBuilderProvider
    {
        public CloseProjectParamsBuilder newCloseProjectParamsBuilder()
        {
            return new CloseProjectParamsBuilder();
        }

        public StartRecordingParamsBuilder newStartRecordingBuilder()
        {
            return new StartRecordingParamsBuilder();
        }

        public StopRecordingParamsBuilder newStopRecordingBuilder()
        {
            return new StopRecordingParamsBuilder().frameworkParameterSearch(true);
        }

        public UpdateUserPathParamsBuilder newUpdateUserPathParamsBuilder()
        {
            return new UpdateUserPathParamsBuilder().deleteRecording(true);
        }
    }
}
