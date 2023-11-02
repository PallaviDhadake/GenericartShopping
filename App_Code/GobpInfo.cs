using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.IO;

/// <summary>
/// Summary description for GobpInfo
/// </summary>
public class GobpInfo
{
    iClass c = new iClass();

	public int primaryId { get; set; }
	public DateTime JoinDate { get; set; }
	public DateTime BirthDate { get; set; }
	public string EncryptID { get; set; }
	public int TypeId { get; set; }
	public string TypeFirm { get; set; }
	public string ShopName { get; set; }
	public string ApplicantName { get; set; }
	public int Age { get; set; }
	public string MaritalStatus { get; set; }
	public string EmailId { get; set; }
	public string MobileNo { get; set; }
	public string WhatsAppNo { get; set; }
	public string Address { get; set; }
	public int State { get; set; }
	public int District { get; set; }
	public string City { get; set; }
	public string OBPUserID { get; set; }
	public string UserPassWord { get; set; }
	public float SalesIncentive { get; set; }
	public float OBPReferral { get; set; }
	public string OwnerEducation { get; set; }
	public string OwnerOccupation { get; set; }
	public string ProfilePhoto { get; set; }
	public string AddressProof1 { get; set; }
	public string AddressProof2 { get; set; }
	public string IdProof1 { get; set; }
	public string IdProof2 { get; set; }
	public string Resume { get; set; }
	public string LegalMatter { get; set; }
	public string ResidenceFrom { get; set; }
	public string UTRNumber { get; set; }
	public string BankName { get; set; }
	public DateTime TransDate { get; set; }

	public object objTransDate = null;

	public string AccountHolder { get; set; }
	public float PaidAmmount { get; set; }
	public int IsClosed { get; set; }
	public float TotalPurchase { get; set; }
	public string Remark { get; set; }
	public string ShopCode { get; set; }
	public string StatusFlag { get; set; }
	public int OBPDHID { get; set; }
	public int OBPZHID { get; set; }
	public string BankAccType { get; set; }
	public string BankNameInfo { get; set; }
	public string BankAccByName { get; set; }
	public string BankAccNumber { get; set; }
	public string BankIFSC { get; set; }
	public string OBPDH_UserId { get; set; }
	public int IsMLM { get; set; }
	public int JoinLevel { get; set; }
	public float CorpCommission { get; set; }
	public string ProfilePhotoPath { get; set; }
	public string ResumePath { get; set; }
	public string Idproof1Path { get; set; }
	public string Idproof2Path { get; set; }
	public string AddressProof1Path { get; set; }
	public string AddressProof2Path { get; set; }
	public string DistrictHeadName { get; set; }
	public string OBPZHUserId{ get; set; }
	public string OBPRefUserId { get; set; }

	public string YearlyGOBPSummary { get; set; }

	public string imagePath;

	
	public GobpInfo()
    {
        //
        // TODO: Add constructor logic here
        //
    }

	public void OBPData(int OBPIdx)
	{
		string connectionString = c.OpenConnection();
		string queryString = "SELECT * FROM OBPData WHERE OBP_ID = " + OBPIdx;

		using (SqlConnection connection = new SqlConnection(connectionString))
		{

			SqlCommand command = new SqlCommand(queryString, connection);
			connection.Open();

			SqlDataReader reader = command.ExecuteReader();
			object result = null;

			while (reader.Read())
			{
				primaryId = (reader["OBP_ID"] != DBNull.Value) ? Convert.ToInt32(reader["OBP_ID"]) : 0;
				//EncryptID = reader["OBP_EncryptID"] != DBNull.Value ? reader["OBP_EncryptID"].ToString() : "";
				TypeFirm = reader["OBP_TypeFirm"] != DBNull.Value ? reader["OBP_TypeFirm"].ToString() : "";
				TypeId = (reader["OBP_FKTypeID"] != DBNull.Value) ? Convert.ToInt32(reader["OBP_FKTypeID"]) : 0;
				Age = (reader["OBP_Age"] != DBNull.Value) ? Convert.ToInt32(reader["OBP_Age"]) : 0;
				ShopName = reader["OBP_ShopName"] != DBNull.Value ? reader["OBP_ShopName"].ToString() : "";
				ApplicantName = reader["OBP_ApplicantName"] != DBNull.Value ? reader["OBP_ApplicantName"].ToString() : "";
				MaritalStatus = reader["OBP_MaritalStatus"] != DBNull.Value ? reader["OBP_MaritalStatus"].ToString() : "";
				EmailId = reader["OBP_EmailId"] != DBNull.Value ? reader["OBP_EmailId"].ToString() : "";
				MobileNo = reader["OBP_MobileNo"] != DBNull.Value ? reader["OBP_MobileNo"].ToString() : "";
				WhatsAppNo = reader["OBP_WhatsApp"] != DBNull.Value ? reader["OBP_WhatsApp"].ToString() : "";
				Address = reader["OBP_Address"] != DBNull.Value ? reader["OBP_Address"].ToString() : "";

				SalesIncentive = (reader["OBP_SalesIncent"] != DBNull.Value) ? Convert.ToInt32(reader["OBP_SalesIncent"]) : 0;
				OBPReferral = (reader["OBP_Referral"] != DBNull.Value) ? Convert.ToInt32(reader["OBP_Referral"]) : 0;


				State = (reader["OBP_StateID"] != DBNull.Value) ? Convert.ToInt32(reader["OBP_StateID"]) : 0;
				District = (reader["OBP_DistrictID"] != DBNull.Value) ? Convert.ToInt32(reader["OBP_DistrictID"]) : 0;
				City = reader["OBP_City"] != DBNull.Value ? reader["OBP_City"].ToString() : "";
				OBPUserID = reader["OBP_UserID"] != DBNull.Value ? reader["OBP_UserID"].ToString() : "";

				UserPassWord = reader["OBP_UserPWD"] != DBNull.Value ? reader["OBP_UserPWD"].ToString() : "";
				OwnerEducation = reader["OBP_OwnerEdu"] != DBNull.Value ? reader["OBP_OwnerEdu"].ToString() : "";
				OwnerOccupation = reader["OBP_OwnerOccup"] != DBNull.Value ? reader["OBP_OwnerOccup"].ToString() : "";


				ProfilePhoto = reader["OBP_ProfilePic"] != DBNull.Value ? reader["OBP_ProfilePic"].ToString() : "";

				AddressProof1 = reader["OBP_AddProof1"] != DBNull.Value ? reader["OBP_AddProof1"].ToString() : "";
				AddressProof2 = reader["OBP_AddProof2"] != DBNull.Value ? reader["OBP_AddProof2"].ToString() : "";

				IdProof1 = reader["OBP_IDProof1"] != DBNull.Value ? reader["OBP_IDProof1"].ToString() : "";
				IdProof2 = reader["OBP_IDProof2"] != DBNull.Value ? reader["OBP_IDProof2"].ToString() : "";


				Resume = reader["OBP_Resume"] != DBNull.Value ? reader["OBP_Resume"].ToString() : "";
				LegalMatter = reader["OBP_LegalMatter"] != DBNull.Value ? reader["OBP_LegalMatter"].ToString() : "";
				ResidenceFrom = reader["OBP_ResidenceFrom"] != DBNull.Value ? reader["OBP_ResidenceFrom"].ToString() : "";
				UTRNumber = reader["OBP_UTRNum"] != DBNull.Value ? reader["OBP_UTRNum"].ToString() : "";
				BankName = reader["OBP_BankName"] != DBNull.Value ? reader["OBP_BankName"].ToString() : "";
				AccountHolder = reader["OBP_AccHolder"] != DBNull.Value ? reader["OBP_AccHolder"].ToString() : "";
				PaidAmmount = (reader["OBP_PaidAmt"] != DBNull.Value) ? Convert.ToInt32(reader["OBP_PaidAmt"]) : 0;
				IsClosed = (reader["OBP_IsClosed"] != DBNull.Value) ? Convert.ToInt32(reader["OBP_IsClosed"]) : 0;
				TotalPurchase = (reader["OBP_TotalPurchase"] != DBNull.Value) ? Convert.ToInt32(reader["OBP_TotalPurchase"]) : 0;
				Remark = reader["OBP_Remark"] != DBNull.Value ? reader["OBP_Remark"].ToString() : "";
				ShopCode = reader["OBP_ShopCode"] != DBNull.Value ? reader["OBP_ShopCode"].ToString() : "";
				StatusFlag = reader["OBP_StatusFlag"] != DBNull.Value ? reader["OBP_StatusFlag"].ToString() : "";


				//OBPDHID = (reader["OBP_DhId"] != DBNull.Value) ? Convert.ToInt32(reader["OBP_DhId"]) : 0;
				OBPZHID = (reader["OBP_ZhId"] != DBNull.Value) ? Convert.ToInt32(reader["OBP_ZhId"]) : 0;
				BankAccType = reader["OBP_BankAccType"] != DBNull.Value ? reader["OBP_BankAccType"].ToString() : "";
				BankNameInfo = reader["OBP_BankNameInfo"] != DBNull.Value ? reader["OBP_BankNameInfo"].ToString() : "";
				BankAccByName = reader["OBP_BankAccByName"] != DBNull.Value ? reader["OBP_BankAccByName"].ToString() : "";
				BankAccNumber = reader["OBP_BankAccNumber"] != DBNull.Value ? reader["OBP_BankAccNumber"].ToString() : "";
				BankIFSC = reader["OBP_BankIFSC"] != DBNull.Value ? reader["OBP_BankIFSC"].ToString() : "";

				OBPDH_UserId = reader["OBP_BankIFSC"] != DBNull.Value ? reader["OBP_BankIFSC"].ToString() : "";

				IsMLM = (reader["IsMLM"] != DBNull.Value) ? Convert.ToInt32(reader["IsMLM"]) : 0;

				JoinLevel = (reader["OBP_JoinLevel"] != DBNull.Value) ? Convert.ToInt32(reader["OBP_JoinLevel"]) : 0;

				//BirthDate = Convert.ToDateTime(reader["OBP_BirthDate"]);

				CorpCommission = (reader["OBP_CorpCommission"] != DBNull.Value) ? Convert.ToInt32(reader["OBP_CorpCommission"]) : 0;


				JoinDate = Convert.ToDateTime(reader["OBP_JoinDate"]);


				objTransDate = (reader["OBP_TransDate"] != DBNull.Value) ? (reader["OBP_TransDate"]) : null;
				//TransDate = (objTransDate != null) ? Convert.ToDateTime(reader["objTransDate"]) :;
				//BirthDate = Convert.ToDateTime(reader["OBP_BirthDate"]);

				ResumePath = HttpContext.Current.Server.MapPath("~/upload/gobpData/resume/") + Resume;
				ProfilePhotoPath = HttpContext.Current.Server.MapPath("~/upload/gobpData/profilePhoto/") + ProfilePhoto;
				AddressProof1Path = HttpContext.Current.Server.MapPath("~/upload/gobpData/addressProof/") + AddressProof1;
				AddressProof2Path = HttpContext.Current.Server.MapPath("~/upload/gobpData/addressProof/") + AddressProof2;
				Idproof1Path = HttpContext.Current.Server.MapPath("~/upload/gobpData/idProof/") + IdProof1;
				Idproof2Path = HttpContext.Current.Server.MapPath("~/upload/gobpData/idProof/") + IdProof2;

				DistrictHeadName = reader["OBP_DH_Name"] != DBNull.Value ? reader["OBP_DH_Name"].ToString() : "";

				OBPZHUserId = reader["OBP_ZH_UserID"] != DBNull.Value ? reader["OBP_ZH_UserID"].ToString() : "";
				OBPRefUserId = reader["OBP_Ref_UserId"] != DBNull.Value ? reader["OBP_Ref_UserId"].ToString() : "";

				// imagePath = Server.MapPath("~/upload/gobpData/addressProof/") + AddressProof1;
				//arrGobpData[15] = "<img src=\"" + imagePath + "\" style=\"width:200px\">";


			}

			reader.Close();

		}
	}

}