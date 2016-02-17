#include "stdafx.h"
#include <iostream>
#include <string>

using std::string;
using std::cout;
using std::cin;
using std::endl;

int main()
{	
	string word;
	cin >> word;

	for (string::size_type i = 0; i < word.size()/2; i++) {
		if (word[i] != word[word.size() - 1 - i]){
			cout <<"Not palindrome"<<endl;
			break;
		}
	}
	system("pause"); 
	cout << "Goodbye" << endl;

    return 0;
}
