namespace HuntroxGames.LD49
{
    public interface IMercenaryDemand
    {
		public string DemandID { get;}
		bool IsMyDemand(string demandID);
	}
}