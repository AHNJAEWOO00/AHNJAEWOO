class Calculator:
    def __init__(self):
        print("계산기를 시작합니다. 'exit'을 입력하면 종료됩니다.")

    def evaluate_expression(self, expression):
        try:
            result = eval(expression)
            return f"결과: {result}"
        except Exception as e:
            return f"오류 발생: {e}"

    def run(self):
        while True:
            print("\n계산할 수식을 입력하세요 (여러 줄 입력 가능, 엔터키를 두번 누르면 계산을 합니다.):")
            lines = []
            while True:
                line = input()
                if line.lower() == "exit":
                    print("계산기를 종료합니다.")
                    return
                if line == "":
                    break
                lines.append(line)

            expression = " ".join(lines)
            if expression.strip() == "":
                print("입력된 수식이 없습니다.")
                continue

            result = self.evaluate_expression(expression)
            print(result)


# 실행
calc = Calculator()
calc.run()
