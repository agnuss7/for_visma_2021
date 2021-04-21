using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
  
namespace dotnet
{
    class Program
    {
		static List<book> read_json() {
			string books_string = File.ReadAllText("files/books.json");
			return JsonConvert.DeserializeObject<List<book>>(books_string);
		}
		
		static void write_json(List<book> list_of_books){
			string json_string = JsonConvert.SerializeObject(list_of_books);
			File.WriteAllText("files/books.json",json_string);
		}
		
		static void add_book(){
			Console.WriteLine("Write the name, author, category, language and ISBN:");
			string[] temporary = new string[5];
			for (int i=0; i<5;i++){
				temporary[i]=Console.ReadLine();
			}
			book Book = new book();
			Book.book_from_array(temporary);
			var all_books = read_json();
			all_books.Add(Book);
			write_json(all_books);
		}
		
		static void borrow_book(){
			string client_name;
			Console.WriteLine("Who are you?");
			client_name = Console.ReadLine();
			var all_books = read_json();
			int clients_borrowed_books=0;
			List<book> books_available = new List<book>();
			List<book> books_unavailable = new List<book>();
			foreach (book n in all_books){
				if (n.Taken){
					books_unavailable.Add(n);
					if (n.Info.Client_Name==client_name){
						clients_borrowed_books+=1;
					}
				} else {
					books_available.Add(n);
				}
			}
			if (books_available.Count>0){
				if (clients_borrowed_books<3){
					Console.WriteLine("What book would you like?");
					for (int i=0;i<books_available.Count;i++){
						Console.WriteLine(i.ToString()+" - "+books_available[i].book_presentation());
					}
					int picked=-1;
					while (picked<0 || picked>=books_available.Count){
						picked = Convert.ToInt32(Console.ReadLine());
						if (picked<0 || picked>=books_available.Count){
							Console.WriteLine("Select a valid option");
						}
					}
					Console.WriteLine("For how many days would you like to borrow the book?");
					int days=-1;
					while (days<0 || days>60){
						days = Convert.ToInt32(Console.ReadLine());
						if(days<0){
							Console.WriteLine("Enter a valid duration");
						} else if (days>60){
							Console.WriteLine("You can't take the book for longer than two months (60 days)");
						}
					}
					books_available[picked].take_out(client_name, days);
					books_available.AddRange(books_unavailable);
					write_json(books_available);
					
				} else {
					Console.WriteLine("You cannot borrow any more books.");
				}
			} else {
				Console.WriteLine("No books available.");
			}
		}
		
		static void return_book(){
			string client_name;
			Console.WriteLine("Who are you?");
			client_name = Console.ReadLine();
			List<book> clients_books = new List<book>();
			List<book> other_books = new List<book>();
			var all_books = read_json();
			foreach (book n in all_books){
				if (n.Taken && n.Info.Client_Name==client_name){
					clients_books.Add(n);
				} else {
					other_books.Add(n);
				}
			}
			if (clients_books.Count>0){
				Console.WriteLine("What book would you like to return?");
				int pick=-1;
				for (int i=0;i<clients_books.Count;i++){
					Console.WriteLine(i.ToString()+" - "+clients_books[i].book_presentation());
				}
				while (pick<0 || pick>=clients_books.Count){
					pick = Convert.ToInt32(Console.ReadLine());
					if(pick<0 || pick>=clients_books.Count){
						Console.WriteLine("Select a valid option");					
					}
				}
				if (!clients_books[pick].return_book()){
					Console.WriteLine("You're late! Off with yer head!");
				}
				clients_books.AddRange(other_books);
				write_json(clients_books);
			}
			else {
				Console.WriteLine("You haven't borrowed any books.");
			}
			
		}
		
		static void list_books(){
			List<book> all_books = read_json(); 
			Console.WriteLine("By what would you like to filter the books?");  
			Console.Write(@"0 - Name
1 - Author
2 - Category
3 - Language
4 - ISBN
5 - Availability
");
			int option=-1;
			while (option<0 || option>5){
				option = Convert.ToInt32(Console.ReadLine());
				if (option<0 || option>5){
					Console.WriteLine("Select a valid option");
				}
			}
			string search="";
			if (option<5){
				Console.WriteLine("Type a search word:");
				search = Console.ReadLine();
			}
			switch (option)
			{
				case 0:
					foreach (book n in all_books){
						if(n.search_by_name(search)){
							Console.WriteLine(n.book_full());
						}
					}
					break;
				case 1:
					foreach (book n in all_books){
						if(n.search_by_author(search)){
							Console.WriteLine(n.book_full());
						}
					}
					break;
				case 2:
					foreach (book n in all_books){
						if(n.search_by_category(search)){
							Console.WriteLine(n.book_full());
						}
					}
					break;
				case 3:
					foreach (book n in all_books){
						if(n.search_by_language(search)){
							Console.WriteLine(n.book_full());
						}
					}
					break;
				case 4:
					foreach (book n in all_books){
						if(n.search_by_isbn(search)){
							Console.WriteLine(n.book_full());
						}
					}
					break;
				case 5:
					int availability=-1;
					Console.WriteLine("0 - available books, 1 - taken books");
					while (!(availability==0 || availability==1)){
						availability = Convert.ToInt32(Console.ReadLine());
					}
					foreach (book n in all_books){
						if (availability==0){
							if (!n.Taken){
								Console.WriteLine(n.book_full());
							}
						} else {
							if (n.Taken){
								Console.WriteLine(n.book_full());
							}
						}
					}
					break;
			}
			
			
		}
		
		
		static void delete_book(){
			var all_books = read_json();
			Console.WriteLine("Which book would you like to burn?");
			for (int i=0;i<all_books.Count;i++){
				Console.WriteLine(i.ToString()+" - "+all_books[i].book_presentation());
			}
			int option=-1;
			while(option<0 || option>=all_books.Count){
				option = Convert.ToInt32(Console.ReadLine());
				if (option<0 || option>=all_books.Count){
					Console.WriteLine("Select a valid option");
				}
			}
			all_books.RemoveAt(option);
			write_json(all_books);
			
		}
		
        static void Main(string[] args)
        {
			int option=-1;
			while (option!=0) {
				Console.WriteLine("What would you like to do?");
				Console.Write(@"0 - Exit
1 - Add a book
2 - Borrow a book
3 - Return a book
4 - List books
5 - Delete book
");
				option=Convert.ToInt32(Console.ReadLine());
				switch (option)
				{
					case 0:
						Console.WriteLine("Thanks for coming! Have a nice day.");
						break;
					case 1:
						add_book();
						break;
					case 2:
						borrow_book();
						break;
					case 3:
						return_book();
						break;
					case 4:
						list_books();
						break;
					case 5:
						delete_book();
						break;
					default:
						Console.WriteLine("Enter a valid option");
						break;
				}
				
				
				
			} 
        }
    }
}
 