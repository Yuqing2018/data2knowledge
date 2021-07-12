# coding=utf-8
# The ApiError class uses to response error status code/text to client.

class ApiError(Exception):
    def __init__(self, message, status_code=500):
        Exception.__init__(self)
        self.message = message
        self.status_code = status_code

    def status_line(self):
        return str(self.status_code) + ' ' + self.message