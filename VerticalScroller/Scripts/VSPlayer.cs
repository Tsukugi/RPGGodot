public partial class VSPlayerHandler {
    int money = 0;

    public int Money { get => money; }

    public void EarnMoney(int amount) {
        money += amount;
    }

    public void SpendMoney(int amount) {
        money -= amount;
    }
}