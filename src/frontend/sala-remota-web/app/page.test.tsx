import { render, screen } from "@testing-library/react";
import { describe, expect, it } from "vitest";
import Home from "./page";

describe("Home", () => {
  it("identifies the technical foundation", () => {
    render(<Home />);

    expect(screen.getByRole("heading", { name: "Sala Remota" })).toBeInTheDocument();
    expect(screen.getByText("Fundação técnica em preparação.")).toBeInTheDocument();
  });
});
