using System;
using BlackSilverUfa_letsplay;
class Program {
 static int assertions;
 static void Equal(string expected,string actual) { assertions++; if(expected!=actual) throw new Exception("Expected "+expected+"; got "+actual); }
 static void Main() {
  var page=new ItemDetailPage(); string id="AbCdEf12_-3";
  Equal(id,page.ParseForTest("https://www.youtube.com/watch?feature=video&v="+id));
  Equal(id,page.ParseForTest("https://www.youtube.com/watch?v="+id));
  Equal(id,page.ParseForTest("http://www.youtube.com/watch?feature=player_embedded&v="+id+"&other=1"));
  Equal(id,page.ParseForTest("https://www.youtube.com/watch?V="+id+"#t=3"));
  Equal(id,page.ParseForTest(id));
  foreach(string invalid in new[]{"", "https://www.youtube.com/watch?prev="+id, "https://www.youtube.com/watch?v=short", "https://www.youtube.com/watch?v="+id+"extra", "https://www.youtube.com/watch?v=bad!bad!bad"}) Equal(invalid,page.ParseForTest(invalid));
  Console.WriteLine("PASS "+assertions+" actual-page URL parsing cases; no network or playback");
 }
}
