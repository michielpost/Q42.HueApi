using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Q42.HueApi.Extensions
{
  public static class CancellationTokenSourceExtensions
  {
    /// <summary>
    /// Based on: https://stackoverflow.com/questions/31495411/a-call-to-cancellationtokensource-cancel-never-returns/31496340#31496340
    /// </summary>
    /// <param name=""></param>
    public static void CancelWithBackgroundContinuations(this CancellationTokenSource cancellationTokenSource)
    {
      Task.Run(() => cancellationTokenSource.Cancel());
      cancellationTokenSource.Token.WaitHandle.WaitOne(); // make sure to only continue when the cancellation completed (without waiting for all the callbacks)
    }
  }
}
