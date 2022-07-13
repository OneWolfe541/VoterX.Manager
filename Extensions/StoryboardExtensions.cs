using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace VoterX.Extensions
{
    // https://stackoverflow.com/questions/14690960/wpf-what-is-the-correct-way-to-wait-for-a-storyboard-animation-to-complete-befo
    public static class StoryboardExtensions
    {
        public static Task BeginAsync(this Storyboard storyboard)
        {
            System.Threading.Tasks.TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            if (storyboard == null)
                tcs.SetException(new ArgumentNullException());
            else
            {
                EventHandler onComplete = null;
                onComplete = (s, e) => {
                    storyboard.Completed -= onComplete;
                    tcs.SetResult(true);
                };
                storyboard.Completed += onComplete;
                storyboard.Begin();
            }
            return tcs.Task;
        }
    }
}
