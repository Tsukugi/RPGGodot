using System.Timers;

public static class TimerUtils {
    public static Timer CreateSimpleTimer(ElapsedEventHandler onElapsed, double durationInSeconds) {
        Timer step = new(durationInSeconds * 1000);
        step.Elapsed -= onElapsed;
        step.Elapsed += onElapsed;
        step.AutoReset = false;
        step.Enabled = true;
        return step;
    }
    public static Timer CreateInterval(ElapsedEventHandler onElapsed, double durationInSeconds) {
        Timer step = new(durationInSeconds * 1000);
        step.Elapsed -= onElapsed;
        step.Elapsed += onElapsed;
        step.Enabled = true;
        return step;
    }
}