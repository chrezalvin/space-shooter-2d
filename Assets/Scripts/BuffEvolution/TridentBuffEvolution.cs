public class TridentBuffEvolution : EvolutionSystem
{
    private BuffManager m_buffManager;
    private BuffDatabase m_buffDatabase;

    public override void Evolve(BuffManager buffManager, BuffDatabase buffDatabase)
    {
        m_buffManager = buffManager;
        m_buffDatabase = buffDatabase;

        BuffManager.OnBuffAdded += HandleBuffAddedTrident;

        m_buffManager.AddBuff(m_buffDatabase.GetBuff(BuffType.TRIDENT_EVOLVE), 0f, true);
    }

    public override BuffType GetBuffType()
    {
        return BuffType.TRIDENT_EVOLVE;
    }

    private void HandleBuffAddedTrident(ActiveBuff activeBuff)
    {
        return;
    }

    public override void OnDestroy()
    {
        BuffManager.OnBuffAdded -= HandleBuffAddedTrident;
    }

    public override void OnDisable()
    {
        BuffManager.OnBuffAdded -= HandleBuffAddedTrident;
    }
}
