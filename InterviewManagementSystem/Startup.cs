using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InterviewManagementSystem.Startup))]
namespace InterviewManagementSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
