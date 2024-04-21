# Template Tokens

Template tokens can be used to replace certain values in the export path.

They exist in several forms:
- simple form: `{token}` where `token` is the token name
- with default: `{token,=default}` where `token` is the token name and `default` is the default value
- with format: `{token,:format}` where `token` is the token name and `format` is the format string
- with default and format: `{token,=default,:format}` where `token` is the token name, `default` is the default value, and `format` is the format string
