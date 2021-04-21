using System;

namespace dotnet
{
    class book
    {
		public string Name {get; set;}
		public string Author {get; set;}
		public string Category {get; set;}
		public string Language {get; set;}
		public string ISBN {get; set;}
		public bool Taken {get; set;}
		public taken_book_info Info {get; set;}
		
		public void book_from_array (string[] tmp){
			Name=tmp[0];
			Author=tmp[1];
			Category=tmp[2];
			Language=tmp[3];
			ISBN=tmp[4];
			Taken=false;
			Info = new taken_book_info();
			Info.Client_Name="";
			Info.Date_Due=new DateTime(2015, 12, 25); 
		}
		
		public void take_out(string c_name, int duration){
			Taken=true;
			Info.Client_Name = c_name;
			DateTime now = DateTime.Now;
			DateTime dt = now.AddDays(duration);
			Info.Date_Due=dt;
			
		}
		
		public bool return_book(){
			Taken=false;
			if (Info.Date_Due.Ticks>(DateTime.Now).Ticks)	{
				return true; 
			}
			else {
				return false;
			}
		}
		
		public bool search_by_name(string s){
			if ((Name.ToLower()).Contains(s.ToLower())){
				return true;
			} else {
				return false;
			}
		}
		
		public bool search_by_author(string s){
			if ((Author.ToLower()).Contains(s.ToLower())){
				return true;
			} else {
				return false;
			}
		}
		
		public bool search_by_category(string s){
			if ((Category.ToLower()).Contains(s.ToLower())){
				return true;
			} else {
				return false;
			}
		}
		
		public bool search_by_language(string s){
			if ((Language.ToLower()).Contains(s.ToLower())){
				return true;
			} else {
				return false;
			}
		}
		
		public bool search_by_isbn(string s){
			if ((ISBN.ToLower()).Contains(s.ToLower())){
				return true;
			} else {
				return false;
			}
		}
		
		public string book_presentation(){
			return (Name + " by " + Author+"\n");
		}
		
		public string book_full(){
			return ("Name: "+Name + "\tAuthor: "+Author+"\tCategory: "+Category+"\tLanguage: "+Language+"\tISBN: "+ISBN+"\n");
		}
	}
	
	class taken_book_info
	{
		public string Client_Name {get; set;}
		public DateTime Date_Due {get; set;}
		
	}
}
