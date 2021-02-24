from math import ceil
from itertools import combinations
import time     # to check runtime

MIN_SUPPORT_PERCENT=0.3

# This function reads a file under filename and extracts all transactions and a set of distinct items
# param filename: The name of the input file (should provide path if necessary)
# return: A dictionary of transactions and a set of distinct items
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

# This function calculates support of the itemset from transactions
# param transactions: All transactions in a dictionary
# param itemset: The itemset to calculate support
# return: The support count of the itemset
def support(transactions, itemset):
    frequent = 0
    iset = set()
    if len(itemset) == 1:
        iset = itemset  # if itemset size == 1, just use them. iset is standard set for using 'issubset()'
    else:
        for i in itemset:   # i:<str> iset:{set}
            iset.add(i)

    for key in transactions.keys():
        vset = set()    # vset is making transactions' value to set
        for i in transactions[key]:
            vset.add(i)
        if iset.issubset(vset):  # count for each transaction
            frequent = frequent + 1

    support_count = frequent
    return support_count

# This function generates a combination from the frequent itemsets of size (itemset_size - 1) and accepts joined itemsets if they share (itemset_size - 2) items
# param frequent_itemsets: The table of frequent itemsets discovered
# param itemset_size: The size of joined itemsets
# return: All valid joined itemsets
def generate_selectively_joined_itemsets(frequent_itemsets, itemset_size):
    # Record seen_itemsets to prevent duplicates
    seen_itemsets = set()
    joined_itemsets = set()
    # ------------------------------------------------------------------------------------------------------------------------------------
    for i in frequent_itemsets[itemset_size-1]:     # i : <str>
        seen_itemsets.add(i)     # itemset_size-1 's values(item) are joined 'seen_itemsets'(set)

    combset = set(combinations(list(seen_itemsets), itemset_size))    # combinate size= i temset_size, combinate seen_itemsets

    joined_itemsets = combset   # generate bombination set, we will prune this set after this fuction act
    # ------------------------------------------------------------------------------------------------------------------------------------
    """
    Try all combinations of two itemsets from the table of frequent itemsets and join the pair if they share (itemset_size - 2) items
    Add each joined itemset to the list if it is not present in the list and discard it otherwise
    """
    return joined_itemsets

# This function checks all the subsets of selected itemsets whether they all are frequent or not and prunes the itemset if anyone of the subsets is not frequent
# param selected_itemsets: The itemsets which are needed to be checked
# param frequent_itemsets: The table of frequent itemsets discovered
# param itemset_size: The size of intended frequent itemsets
# return: The itemsets whose all subsets are frequent
def apply_apriori_pruning(selected_itemsets, frequent_itemsets, itemset_size):
    apriori_pruned_itemsets = set()
    # ------------------------------------------------------------------------------------------------------------------------------------
    if itemset_size == 2:
        apriori_pruned_itemsets = selected_itemsets    # combination set == apriori when size = 2
        return apriori_pruned_itemsets

    for ns in selected_itemsets:   # we should prune with combination set(selected_itemset)
        tmpset = set()      # make temporary set to make union set with item of combination set
        for item in ns:     # ns is consist of items(N(items)== item_size)
            tmpset = tmpset | set(item)     # union them
        if len(tmpset) == itemset_size:     # select the set which size is same as itemset_size
            apriori_pruned_itemsets.add(tuple(tmpset))
    # ------------------------------------------------------------------------------------------------------------------------------------
    """
    Add each itemset to the list if all of its subsets are frequent and discard it otherwise
    """
    return apriori_pruned_itemsets


# This function generates candidate itemsets of size (itemset_size) by selective joining and apriori pruning
# param frequent_itemsets: The table of frequent itemsets discovered
# param itemset_size: The size of intended frequent itemsets
# return: candidate itemsets formed by selective joining and apriori pruning
def generate_candidate_itemsets(frequent_itemsets, itemset_size):
    joined_itemsets = generate_selectively_joined_itemsets(frequent_itemsets, itemset_size)
    candidate_itemsets = apply_apriori_pruning(joined_itemsets, frequent_itemsets, itemset_size)
    return candidate_itemsets

# This function generates a table of itemsets with all frequent items from transactions based on a given minimum support
# param transactions: The transactions based upon which support is calculated
# param items: The unique set of items present in the transaction
# param min_support: The minimum support to find frequent itemsets
# return: The table of all frequent itemsets of different sizes
def generate_all_frequent_itemsets(transactions, items, min_support):

    frequent_itemsets = dict()
    itemset_size = 0    #starting with 0
    frequent_itemsets[itemset_size] = list()
    frequent_itemsets[itemset_size].append(frozenset()) #Frozen set can not fix

    # Frequent itemsets of size 1 **
    itemset_size += 1
    frequent_itemsets[itemset_size] = list() #key:1, frequent_itemsets[key]=values (type: list)
    # ------------------------------------------------------------------------------------------------------------------------------------
    tmpdict = dict()
    tmpset = set()
    for item in items:
        tmpset.add(item)

        if support(transactions, tmpset) >= min_support:    # support rule
            tmpdict[item] = support(transactions, tmpset)   # make dictionary which matched item-support structure
        tmpset.clear()      # if we use tmpset for one item, clear it for recycle
    frequent_itemsets[itemset_size] = tmpdict   # match for return name
    """
    Find all frequent itemsets of 1 and add them to the list
    """
    # ------------------------------------------------------------------------------------------------------------------------------------
    # frequent itemsets of greater size
    itemset_size += 1

    # generate (frequent, candidate) itemset
    while frequent_itemsets[itemset_size - 1]:
        frequent_itemsets[itemset_size] = list()
        candidate_itemsets = generate_candidate_itemsets(frequent_itemsets, itemset_size)
        pruned_itemset = set()
        # ------------------------------------------------------------------------------------------------------------------------------------
        tempdict2 = dict()  # we use tmpdict above so use tmpdict2 to avoid confuse

        for i in candidate_itemsets:
            if support(transactions, i) >= min_support:     # support rule
                tempdict2[i] = support(transactions, i)     # make dictionary as same as tmpdict

        pruned_itemset = tempdict2  # pruning with support rule
        # ------------------------------------------------------------------------------------------------------------------------------------
        """
        Prune the candidate itemset if its support is less than minimum support
        """
        frequent_itemsets[itemset_size] = pruned_itemset
        itemset_size += 1


    return frequent_itemsets

def find_key(dict,val):
    return next(key for key, value in dict.items() if value==val)

def output_to_file(filename,frequent_itemsets_table,transactions):
    file = open(filename, 'w',encoding='UTF8')
    Max=0
    temp=''
    for item in frequent_itemsets_table[1]:  # size 1 items

        ttem = set()
        ttem.add(item)
        string = str() + '{ '
        string = string + str(item) + ', '
        string = string[:-2] + ' }  ' + str(
            round(support(transactions, ttem) / len(transactions) * 100, 1)) + "% support"
        file.write(string + "\n")

        if(round(support(transactions, ttem) / len(transactions) * 100, 1)>Max):
            Max=round(support(transactions, ttem) / len(transactions) * 100, 1)
            temp=ttem

    print(Max)
    print(temp)


    for i in range(2, len(frequent_itemsets_table)):  # the other
        for table in frequent_itemsets_table[i]:
            Max = 0
            string = str() + '{ '
            for item in table:
                string = string + str(item) + ', '
            string = string[:-2] + ' }  ' + str(
                round(support(transactions, table) / len(transactions) * 100, 1)) + "% support"
            file.write(string + "\n")

            if (round(support(transactions, ttem) / len(transactions) * 100, 1) > Max):
                Max = round(support(transactions, ttem) / len(transactions) * 100, 1)
                temp = str(item)+string[:-2]
    #print(Max)
    #print(temp)

    file.close()


def main():
    start=time.time()
    input_filename='sample.txt'
    output_filename='result.txt'
    transaction, attSet = get_input_data(input_filename)
    min_support = ceil(MIN_SUPPORT_PERCENT * len(transaction))
    frequent_itemsets_table = generate_all_frequent_itemsets(transaction, attSet, min_support)
    output_to_file(output_filename, frequent_itemsets_table, transaction)
    print(f"Done. Runtime: {round(time.time() - start, 2)}sec")

if __name__=='__main__':
    main()