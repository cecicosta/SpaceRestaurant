using UnityEngine;
using System.Collections;

public class Employee{
	public Employee(){}

	public string name;
	public int salary;
	public int level;
	public int hapyness;
	public Occupation type;

	public enum Occupation{Chef, Waiter, Marketing, Finances};
}
