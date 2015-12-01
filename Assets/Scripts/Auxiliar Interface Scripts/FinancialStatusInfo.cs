using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FinancialStatusInfo : MonoBehaviour {
	public Text total;
	public Text income;
	public Text spend;
	public Text clientsReceived;
	public Text clientsAttended;
	public Text currentCapacity;

	private EstablishmentManagement est_mgr;
	private Establishment establishment;
	// Use this for initialization
	void Awake () {
		est_mgr = EstablishmentManagement.GetInstance ();
		if (est_mgr == null)
			Debug.LogError ("EstablishmentManager object is null");
		establishment = est_mgr.establishment;
		if (establishment == null)
			Debug.LogError ("Establishment object is null");
	}

	void OnEnable() {
		total.text = est_mgr.previous_day_cash.ToString();
		income.text = est_mgr.day_income.ToString ();
		spend.text = est_mgr.management_costs.ToString ();
		clientsReceived.text = est_mgr.total_request_number.ToString ();
		clientsAttended.text = est_mgr.attended_requests_number.ToString ();
		currentCapacity.text = est_mgr.current_capacity.ToString ();
	}
}
