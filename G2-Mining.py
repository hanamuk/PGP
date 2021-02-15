"""
dept. Computer Telecommunication Engineering
Assignment No.2 (Charm Algorithm)
"""

from math import ceil
import time     # to check runtime

MIN_SUPPORT_PERCENT = 0.3

### read from external path file
def get_input_data(filename):
    input_file=open(filename,'r',encoding='UTF8')
    transactions=dict()
    itemset=set()
    i = 0
    for line in input_file:

        preTransaction =line.split()
        transactions[i]=preTransaction[0:]
        i+=1

        for item in preTransaction[0:2]:
            itemset.add(item)

    return transactions, itemset

### this fuction is count for each item in all transactions (frequency)
def support(transactions, itemset):
    support_count = 0
    if type(itemset) == type(' '):
        tmp = set()
        tmp.add(itemset)
        itemset = tmp.copy()
    else:
        itemset = set(itemset)

    for key in transactions.keys():
        if itemset.issubset(transactions[key]):
            support_count += 1
    return support_count

def killSets(frequent_itemsets, itemset, itemset_size): # pruned itemset where set has superset
    if itemset in frequent_itemsets[itemset_size-1]:
        del (frequent_itemsets[itemset_size-1][itemset])


### this function select each transaction which contains item
def fallowTransaction(transaction, item):
    itemset=set()   # set of item (adapt to any size of item)
    # type of size 1: str, type over size 2: tuple
    if type(item) == tuple:
        for Ltem in list(item):
            itemset.add(Ltem)
    else:
        itemset.add(item)

    transet=set()
    for k in transaction.keys():
        S=set(transaction[k])

        if itemset.issubset(S):
            transet.add(k)
        S.clear()

    return transet

### The key of charm algorithm, using pruning strategy for each case
def generate_closedSets(transactions, frequent_itemsets, itemset_size):
    closedSet = set()
    frequent_list = list(frequent_itemsets[itemset_size-1].keys())
    checkedtem = set()

    # we have to compare each Transaction of itemset to apply pruning strategy
    for A in frequent_list:
        if A in checkedtem:
            continue
        checkedtem.add(A)
        Atran = fallowTransaction(transactions, A)
        setA=set()  # for compare intersection of A with B
        for at in Atran:
            setA.add(at)

        for B in frequent_list:
            if B in checkedtem:
                continue
            Btran=fallowTransaction(transactions, B)
            setB=set()  # for compare intersection of B with A
            for bt in Btran:
                setB.add(bt)

            ### pruning strategy
            if setA == setB:    # If both of transaction of itemset are same, replace each itemset with union set
                # same man
                killSets(frequent_itemsets,A,itemset_size)
                killSets(frequent_itemsets,B,itemset_size)
                checkedtem.add(B)  # B is over. Do not seek this for compare

                tmpsetA=set()
                tmpsetA.add(A)
                tmpsetB=set()
                tmpsetB.add(B)
                closedSet.add(tuple(tmpsetA.union(tmpsetB)))

            elif setA.issubset(setB):   # If one transaction of itemset is superset of the other, replace subset with union set
                # A is small man
                killSets(frequent_itemsets,A,itemset_size)

                tmpsetA = set()
                tmpsetA.add(A)
                tmpsetB = set()
                tmpsetB.add(B)
                closedSet.add(tuple(tmpsetA.union(tmpsetB)))

            elif setB.issubset(setA):
                # B is small man
                killSets(frequent_itemsets,B,itemset_size)
                checkedtem.add(B)     # B is over. Do not seek this for compare

                tmpsetA = set()
                tmpsetA.add(A)
                tmpsetB = set()
                tmpsetB.add(B)
                closedSet.add(tuple(tmpsetA.union(tmpsetB)))

            else:   # If both transaction of itemset are not same, keep both of them then just add union set
                tmpsetA = set()
                tmpsetA.add(A)
                tmpsetB = set()
                tmpsetB.add(B)
                ElseSet=tuple(tmpsetA.union(tmpsetB))
                if len(ElseSet) == itemset_size:
                    closedSet.add(ElseSet)


    if itemset_size > 2:
        closedSet = list(closedSet)
        for row in closedSet:
            tmp_row = set()
            for i in row:
                tmp_row = tmp_row | set(i)      # union both of them
            closedSet[closedSet.index(row)] = tuple(tmp_row)
    closedSet = set(closedSet)

    return closedSet

# apply min support rule and work recursively (while)
def generate_all_closed_itemsets(transactions, items, min_support):

    frequent_itemsets = dict()
    itemset_size = 0    #starting with 0
    frequent_itemsets[itemset_size] = list()
    frequent_itemsets[itemset_size].append(frozenset()) #Frozen set can not fix

    # Frequent itemsets of size 1 **
    itemset_size += 1
    frequent_itemsets[itemset_size] = list() #key:1, frequent_itemsets[key]=values (type: list)
    tmpdict = dict()

    for item in items:
        if support(transactions, item) >= min_support:    # support rule
            tmpdict[item] = support(transactions, item)   # make dictionary which matched item-support structure
    frequent_itemsets[itemset_size] = tmpdict   # match for return name
# Find all frequent itemsets of 1 and add them to value of frequent_itmesets[1] .. 1:{item:frequent, ...}

    # frequent itemsets of greater size
    itemset_size += 1

    # generate (frequent, candidate) itemset
    while frequent_itemsets[itemset_size - 1]:
        frequent_itemsets[itemset_size] = list()
        closedSets = generate_closedSets(transactions, frequent_itemsets, itemset_size)

        pruned_itemset = dict()

        for i in closedSets:
            if(type(i) == tuple):   # to colander str in ClosedSet( tuple's element)
                if support(transactions, i) >= min_support:  # support rule i or I
                    pruned_itemset[i] = support(transactions, i)  # make dictionary as same as tmpdict

        frequent_itemsets[itemset_size] = pruned_itemset
        itemset_size += 1

    return frequent_itemsets

### write to external path
def output_to_file(filename, frequent_itemsets_table, transactions):
    file = open(filename, 'w',encoding='UTF8')
    Max=0
    ttemp=''
    for i in range(2, len(frequent_itemsets_table)):    # the other
        for table in frequent_itemsets_table[i]:
            string = str() + '{ '
            for item in table:
                string = string + str(item) + ', '
            string = string[:-2] + ' }  ' + str(round(support(transactions, table)/len(transactions)*100,1)) + "% support"
            file.write(string+"\n")

            if(round(support(transactions, table)/len(transactions)*100,1)>Max):
                Max=round(support(transactions, table)/len(transactions)*100,1)
                ttemp=string[0:-14]


    print(Max)
    print(ttemp)
    file.close()
def main():
    start = time.time()
    input_filename = 'sample.txt'
    output_filename = 'result(2).txt'
    transactions, item_set = get_input_data(input_filename)
    min_support = ceil(MIN_SUPPORT_PERCENT * len(transactions))
    frequent_itemsets_table = generate_all_closed_itemsets(transactions, item_set, min_support)
    output_to_file(output_filename, frequent_itemsets_table, transactions)
    print(f"Done. Runtime: {round(time.time() - start, 2)}sec")


if __name__ == '__main__':
    main()
