using System.Windows;
using System.Windows.Controls;

namespace Diary4CuratorFullEdition.Auxiliary.Classes
{
	public static class BorderVariables
	{
		public static Window win;
		public static Frame frm;
		public static Page pg;
	}

	public static class SenderEmailVariables
	{
		public static int code;
	}
	
	public static class AutorizationVariables
	{
		public static int AutorizationId = 1;
	}

	public static class IdVariables
	{
		public static int EventId;

		public static int StudentId;
		public static int HomeAddressId;
		public static int FamilyId;
		public static int SocialMediaId;
		public static int PhoneConversationsId;

		public static int PassportId;
		public static int PlaceRegistrationId;
		public static int MedicalPolicyId;
		public static int KDNiZPId;
		public static int StateAidId;
		public static int ITUId;
	}

	public static class Checkers 
	{
		public static bool DataChanged = false;			//Изменение данных в полях
		public static bool DataTableChanged = false;	//Изменение уже существующих данных
		public static bool NewStudents = false;			//Добавление нового студента
	}
	
	public static class GroupVariables
	{
		public static int GroupId;
		public static int GroupSelected;
	}
}
