using System.Timers;

public class AttackHandler {
	Timer onAttackAnimationTimer;
	Timer onAttackCooldownTimer;

	public Timer OnAttackCooldownTimer { get => onAttackCooldownTimer; }
	public Timer OnAttackAnimationTimer { get => onAttackAnimationTimer; }

	public void StartAttack(ElapsedEventHandler OnAttackStateHandler, ElapsedEventHandler OnAttackCooldownHandler, double attackDuration, double attackCooldownDuration) {
		onAttackAnimationTimer = new Timer(attackDuration * 1000);
		// Hook up the Elapsed event for onAttackStateTimer. 
		onAttackAnimationTimer.Elapsed -= OnAttackStateHandler;
		onAttackAnimationTimer.Elapsed += OnAttackStateHandler;
		onAttackAnimationTimer.AutoReset = false;
		onAttackAnimationTimer.Enabled = true;

		onAttackCooldownTimer = new Timer(attackCooldownDuration * 1000);
		// Hook up the Elapsed event for onAttackCooldownTimer. 
		onAttackCooldownTimer.Elapsed -= OnAttackCooldownHandler;
		onAttackCooldownTimer.Elapsed += OnAttackCooldownHandler;
		onAttackCooldownTimer.AutoReset = false;
		onAttackCooldownTimer.Enabled = true;
	}

}
