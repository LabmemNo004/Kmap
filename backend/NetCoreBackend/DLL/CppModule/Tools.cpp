//
// Created by 谭忠煜 on 2021/7/2.
//

#include "Tools.h"

bool Tools::judgePerson(int score) {
    int scoreline=100000;
    if(score>=scoreline){
        return true;
    }
    else{
        return false;
    }
}

bool Tools::judgeOrganization(int score) {
    int scoreline=2000;
    if(score>=scoreline){
        return true;
    }
    else{
        return false;
    }
}
