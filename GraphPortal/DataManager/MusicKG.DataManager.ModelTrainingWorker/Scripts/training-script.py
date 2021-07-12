import time
import os
import sys

if __name__ == '__main__':
	print("start training model using folder {}...".format(sys.argv[1]))
	time.sleep(30)
	print("start create trained model folder {}...".format(sys.argv[2]))
	os.makedirs(sys.argv[2])
	print("Model is trained on {}".format(time.time()))
